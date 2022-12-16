Shader "zegraber/HeightMapPhong"
{
    SubShader
    {
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texUV : TEXCOORD0;
            };

            struct v2f
            {
                float4 screenPos : SV_POSITION;
                float2 texUV : TEXCOORD0;
                float2 perlinUV : TEXCOORD1;
                float4 worldPos : TEXCOORD2; // A hacky way to pass world pos to fragment shader
                float4 color : COLOR;
                float4 objPos : NORMAL; // Need this to find new normal after height mapping
            };

            // Uniforms for model, view, and projection
            float4x4 _TranslationMatrix, _RotationMatrix, _ScalingMatrix;
            float4x4 _ViewingMatrix, _ProjectionMatrix;

            // Uniforms for lighting info
            // Supports up to 64 lights in a scene
            int _NumLights;
            float3 _CameraPos;
            float3 _LightPos[64];
            float4 _LightDiffuseColor[64], _LightSpecularColor[64];
            float4 _MaterialColor;
            float _LightAmbient, _Specularity, _DiffuseIntensity, _LightPower[64];

            // Textures and maps
            sampler2D _AlphaHeightMap, _MainTex; // (r,g,b,a), where (r,g,b) are a regular texture and alpha channel is height map

            // Uniforms for Perlin sampling info (UV generation)
            float _HeightScale, _PerlinScale;
            float2 _PerlinOffset;

            // function declarations
            float4x4 trs();
            float4 objectNormalToWorld(float4 objNormal);
            float2 heightMapUV(float3 pos);

            v2f vert(appdata v)
            {
                v2f o;
                // Sample Perlin noise texture to change object-space height of mesh
                //------------------------------------------------------------------

                // Sample the height map and apply height change to vertex
                float2 perlinUV = heightMapUV(v.vertex.xyz);
                float4 heightMapSample = tex2Dlod(_AlphaHeightMap, float4(perlinUV, 0.0, 0.0));
                float4 newVert = float4(v.vertex.x, (heightMapSample.a * _HeightScale * 2) - _HeightScale, v.vertex.z, 1.0);

                // Do regular projection pipeline junk
                //------------------------------------------------------------------
                o.screenPos = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(trs(), newVert)));
                o.screenPos.y = -1.0f * o.screenPos.y;
                o.screenPos.z = 1;

                // Pass things along to the fragment shader
                //------------------------------------------------------------------
                o.color = _MaterialColor;
                o.objPos = newVert; // Set position in object space
                o.worldPos = mul(trs(), newVert);
                o.perlinUV = perlinUV;
                o.texUV = v.texUV;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Calculate a new normal vector in object space then convert to world space
                //------------------------------------------------------------------------------
                // 1. Get vectors pointing to nearby points on the surface in the x and z directions

                const float e = 0.05f; // [epsilon]

                // Find nearby X point (sample in the same way as the vertex shader)
                float3 nearbyXPoint = float3(i.objPos.x + e, 0, i.objPos.z);
                float4 nearbyXHeightSample = tex2D(_AlphaHeightMap, heightMapUV(nearbyXPoint));
                nearbyXPoint.y = (nearbyXHeightSample.a * _HeightScale * 2) - _HeightScale;

                // Find nearby Z point
                float3 nearbyZPoint = float3(i.objPos.x, 0, i.objPos.z + e);
                float4 nearbyZHeightSample = tex2D(_AlphaHeightMap, heightMapUV(nearbyZPoint));
                nearbyZPoint.y = (nearbyZHeightSample.a * _HeightScale * 2) - _HeightScale;

                // 2. Take their cross product to find an approximate normal, then normalize it
                const float3 newNormal = cross((nearbyZPoint - i.objPos), (nearbyXPoint - i.objPos));      
                float3 normalVec = (objectNormalToWorld(float4(newNormal, 1.0))).xyz;
                normalVec = normalize(normalVec);     
                // return float4(normalVec, 1); 

                // Sample texture
                //-----------------------------
                float4 color = tex2D(_MainTex, i.texUV);


                // Calculate lighting
                //------------------------

                // Only calculate ambient once, not for each light
                const float4 I_amb =  _LightAmbient * color;

                // Eye vector doesn't change between lights
                const float3 eyeVec = normalize(_CameraPos - i.worldPos.xyz);
                float3 lightVecNotUnit, lightVec, reflVec;
                float attenuation;
                float4 I_diff, I_spec;
                float4 I_total = I_amb;
                for (int l = 0; l < _NumLights; l++) {
                    // Get the vectors we need and normalize them
                    lightVecNotUnit = (_LightPos[l] - i.worldPos.xyz);
                    lightVec = normalize(lightVecNotUnit);
                    reflVec = normalize(((2 * dot(lightVec, normalVec)) * normalVec) - lightVec);

                    // Manually calculating square distance for attenuation saves a sqrt operation :)
                    // square of distance between light and fragment
                    attenuation = pow(lightVecNotUnit.x, 2) + pow(lightVecNotUnit.y, 2) + pow(lightVecNotUnit.z, 2);

                    // Lighting Components
                    I_diff = max(0, dot(normalVec, lightVec)) * _LightPower[l] * _DiffuseIntensity * color * _LightDiffuseColor[l] / attenuation;
                    I_spec = pow(max(0, dot(reflVec, eyeVec)), _Specularity) * _LightPower[l] * color * _LightSpecularColor[l] / attenuation;
                    I_total += I_diff + I_spec; // Add the specular and diffuse this light contributes
                }

                return float4(min(1.0f, I_total.r), min(1.0f, I_total.g), min(1.0f, I_total.b), 1.0f);
            }

            float4x4 trs()
            {
                return mul(_TranslationMatrix, mul(_RotationMatrix, _ScalingMatrix));
            }

            // If we translate normal vectors, bad things happen.
            // Technically these normals would be incorrect for non-uniform scaling
            float4 objectNormalToWorld(float4 objNormal)
            {
                return mul(_RotationMatrix, mul(_ScalingMatrix, objNormal));
            }

            float2 heightMapUV(float3 pos) {
                // Calculate UV on height-map texture based on x and z in object space
                // 0 <= _PerlinScale <= 1, where _PerlinScale == 1 means mesh's UVs cover entire texture
                float2 perlinUV;
                perlinUV.x = _PerlinScale * ((pos.x * 0.5f) + 0.5f) + _PerlinOffset.x;
                perlinUV.y = _PerlinScale * ((pos.z * 0.5f) + 0.5f) + _PerlinOffset.y;
                return perlinUV;
            }

            ENDCG
        }
    }
}
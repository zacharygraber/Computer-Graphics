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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 screenPos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1; // A hacky way to pass world pos to fragment shader
                float4 color : COLOR;
                float4 objPos : NORMAL; // Need this to find new normal after height mapping
            };

            // Uniforms for model, view, and projection
            float4x4 _TranslationMatrix, _RotationMatrix, _ScalingMatrix;
            float4x4 _ViewingMatrix, _ProjectionMatrix;

            // Uniforms for lighting info
            float3 _CameraPos, _LightPos;
            float4 _LightDiffuseColor, _LightSpecularColor, _MaterialColor;
            float _LightIntensity, _LightAmbient, _Specularity, _DiffuseIntensity;

            // Textures and maps
            sampler2D _AlphaHeightMap; // (r,g,b,a), where (r,g,b) are a regular texture and alpha channel is height map
            float _HeightScale;

            // function declarations
            float4x4 trs();
            float4 objectNormalToWorld(float4 objNormal);

            v2f vert(appdata v)
            {
                v2f o;
                float4 heightMapSample = tex2Dlod(_AlphaHeightMap, float4(v.uv, 0.0, 0.0));
                float4 newVert = float4(v.vertex.x, (heightMapSample.a * _HeightScale * 2) - _HeightScale, v.vertex.z, 1.0);
                o.screenPos = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(trs(), newVert)));
                o.screenPos.y = -1.0f * o.screenPos.y;
                o.screenPos.z = 1;
                o.color = _MaterialColor;
                o.objPos = newVert; // Set position in object space
                o.worldPos = mul(trs(), newVert);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Calculate a new normal vector in world space
                // 1. Get vectors pointing to nearby points on the surface in the x and z directions
                float4 nearbyXHeightSample = tex2D(_AlphaHeightMap, float2(i.uv.x + 0.01, i.uv.y));
                const float3 nearbyXPoint = float3(i.objPos.x + 0.01, (nearbyXHeightSample.a * _HeightScale * 2) - _HeightScale, i.objPos.z);
                float4 nearbyZHeightSample = tex2D(_AlphaHeightMap, float2(i.uv.x, i.uv.y + 0.01));
                const float3 nearbyZPoint = float3(i.objPos.x, (nearbyZHeightSample.a * _HeightScale * 2) - _HeightScale, i.objPos.z + 0.01);
                const float3 newNormal = cross((nearbyZPoint - i.objPos), (nearbyXPoint - i.objPos));      
                const float3 normalVec = normalize((objectNormalToWorld(float4(newNormal, 1.0))).xyz);      

                // Calculate lighting
                const float3 lightVec = normalize(_LightPos - i.worldPos.xyz);
                const float3 eyeVec = normalize(_CameraPos - i.worldPos.xyz);
                const float3 reflVec = normalize(((2 * dot(lightVec, normalVec)) * normalVec) - lightVec);
                const float4 I_amb =  _LightAmbient * i.color;
                const float4 I_diff = (clamp(dot(normalVec, lightVec), 0.0, 1.0) * _DiffuseIntensity) * i.color * _LightDiffuseColor;
                const float4 I_spec = (pow(max(0, dot(reflVec, eyeVec)), _Specularity)) * i.color * _LightSpecularColor;

                float4 I_total = I_amb + I_diff + I_spec;
                return float4(min(1.0f, _LightIntensity * I_total.r), min(1.0f, _LightIntensity * I_total.g), min(1.0f, _LightIntensity * I_total.b), 1.0f);
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

            ENDCG
        }
    }
}
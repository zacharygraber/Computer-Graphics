Shader "zegraber/BlinnPhongUntextured"
{
    SubShader
    {

        Pass
        {
            Name "BlinnPhongUntextured"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 screenPos : SV_POSITION;
                float4 worldPos : TEXCOORD0; // A hacky way to pass world pos to fragment shader
                float4 color : COLOR;
                float4 normal : NORMAL;
            };

            // Uniforms for model, view, and projection
            float4x4 _TranslationMatrix, _RotationMatrix, _ScalingMatrix;
            float4x4 _ViewingMatrix, _ProjectionMatrix;

            // Uniforms for lighting info
            float3 _CameraPos, _LightPos;
            float4 _LightDiffuseColor, _LightSpecularColor, _MaterialColor;
            float _LightIntensity, _LightAmbient, _Specularity, _DiffuseIntensity;

            // function declarations
            float4x4 trs();
            float4 objectNormalToWorld(float4 objNormal);

            v2f vert(appdata v)
            {
                v2f o;
                o.screenPos = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(trs(), v.vertex)));
                o.screenPos.y = -1.0f * o.screenPos.y;
                o.screenPos.z = 1;
                o.color = _MaterialColor;
                o.normal = objectNormalToWorld(v.normal);
                o.worldPos = mul(trs(), v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                const float3 normalVec = normalize(i.normal.xyz);
                const float3 lightVec = normalize(_LightPos - i.worldPos.xyz);
                const float3 eyeVec = normalize(_CameraPos - i.worldPos.xyz);
                const float3 reflVec = normalize(((2 * dot(lightVec, normalVec)) * normalVec) - lightVec);
                const float4 I_amb =  _LightAmbient * i.color;
                const float4 I_diff = (clamp(dot(-1*normalVec, lightVec), 0.0, 1.0) * _DiffuseIntensity) * i.color * _LightDiffuseColor;
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
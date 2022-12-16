Shader "zegraber/ColorByObjectNormal"
{
    SubShader
    {
        Pass
        {
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
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            float4x4 _TranslationMatrix;
            float4x4 _RotationMatrix;
            float4x4 _ScalingMatrix;

            float4x4 _ViewingMatrix;
            float4x4 _ProjectionMatrix;

            float4x4 trs();

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(trs(), v.vertex)));
                o.vertex.y = -1.0f * o.vertex.y;
                o.vertex.z = 1;
                o.color = abs(v.normal);
                o.color.w = 1.0;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            float4x4 trs()
            {
                return mul(_TranslationMatrix, mul(_RotationMatrix, _ScalingMatrix));
            }

            ENDCG
        }
    }
}


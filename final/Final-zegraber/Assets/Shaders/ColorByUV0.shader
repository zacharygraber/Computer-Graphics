Shader "zegraber/ColorByUV0"
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
                float2 uv0 : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv0 : TEXCOORD0;
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
                o.vertex.z = 1; // If I don't force the z-depth, for some reason everything renders backward/inside out.
                o.uv0 = v.uv0;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(i.uv0.x, 0.0, i.uv0.y, 1.0);
            }

            float4x4 trs()
            {
                return mul(_TranslationMatrix, mul(_RotationMatrix, _ScalingMatrix));
            }

            ENDCG
        }
    }
}
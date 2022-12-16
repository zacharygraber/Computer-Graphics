Shader "zegraber/PointLightRepr"
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
            };

            float4x4 _TranslationMatrix;
            float4x4 _ScalingMatrix;

            float4x4 _ViewingMatrix;
            float4x4 _ProjectionMatrix;

            float4 _PointLightColor;

            float4x4 ts();

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(ts(), v.vertex)));
                o.vertex.y = -1.0f * o.vertex.y;
                o.vertex.z = 1; // If I don't force the z-depth, for some reason everything renders backward/inside out.
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _PointLightColor;
            }

            float4x4 ts()
            {
                return mul(_TranslationMatrix, _ScalingMatrix);
            }

            ENDCG
        }
    }
}
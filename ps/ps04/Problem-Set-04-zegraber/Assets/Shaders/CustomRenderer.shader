Shader "Unlit/CustomRenderer"
{
    SubShader
    {

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            float4x4 _ModelingMatrix;
            float4x4 _ViewingMatrix;
            float4x4 _ProjectionMatrix;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(_ModelingMatrix, v.vertex)));
                o.vertex.y = -1.0f * o.vertex.y;
                o.vertex.z = 1; // If I don't force the z-depth, for some reason everything renders backward/inside out.
                o.color.xyz = v.normal * 0.5 + 0.5;
                o.color.w = 1.0;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}

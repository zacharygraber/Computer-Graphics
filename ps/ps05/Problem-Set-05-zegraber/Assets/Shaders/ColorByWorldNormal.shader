Shader "zegraber/ColorByWorldNormal"
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
            float4 objectNormalToWorld(float4 objNormal);

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = mul(_ProjectionMatrix, mul(_ViewingMatrix, mul(trs(), v.vertex)));
                o.vertex.y = -1.0f * o.vertex.y;
                o.vertex.z = 1; // If I don't force the z-depth, for some reason everything renders backward/inside out.
                o.color = abs(objectNormalToWorld(v.normal));
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
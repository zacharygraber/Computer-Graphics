Shader "SplineVertexShader" {

    Properties { }
    
    SubShader {
    
        cull off
        
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        
        Pass  {
        
            CGPROGRAM
            
            // this script will provide both a Vertex and a Fragment shader:
            #pragma vertex vert
            #pragma fragment frag
            
            // include Unity built-in shader helper functions:
            #include "UnityCG.cginc"

            // note: in GLSL the following struct of variables
            //     would have the "attribute" qualifier,
            //     as received by the vertex shader
            //     and unique for each vertex shader instance:
            struct appdata {
                float4 vertex : POSITION;
            };

            // note: in GLSL the following struct of variables
            //     would have the "varying" qualifier,
            //     as passed from the vertex shader
            //     to the fragment shader:
            struct v2f {
                float4 vertex : SV_POSITION;
            };
            
            // note: in GLSL the following parameters
            //     would have the "uniform" qualifier:
            
            // here we receive (from application, CPU side) the spline Hermite form matrix:
            float4x4 _SplineMatrix;
            
            // here we receive (from application, CPU side) the four control points:
            float2 _Control0, _Control1, _Control2, _Control3;
            
            // here we receive (from application, CPU side) how many steps there should be on curve:
            float _Step;
            
            

            // ---------------------------------------------------------
            // the spline equation calculation happens here, i.e.:
            // you need to compute the spline multiplication as from the matrix form
            // as per Problem Set 03 instructions,
            // in the form p(u) = uT * M * p
            //   
            //   Note: matrices are defined in "column major" !
            //
            //   Note2: here we name the parameter t instead of u
            //
            float4 HermiteMult(float4x4 controlP, float4x4 splineM, float4 tParamsVector) {
                //
                // compute vertex on curve:
                //
                float4 vertexOnCurve = mul(mul(controlP, splineM), tParamsVector);
                return vertexOnCurve;
            } // end of HermiteMult()



            // ---------------------------------------------------------
            // compute the Normal to the curve, at vertex provided by parameter t:
            // 
            float2 GetNormalToCurve(float t, float dt, float4x4 controlMatrix, float4x4 splineMatrix) {
                //
                // compute normal to segment (p1, p2) on curve
                //        where p1 is the vertex on curve at parameter t
                //        where p2 is the vertex on curve at parameter t2 = t + dt
                //
                // ADDITIONAL NOTE:
                //      The resulting normal should be normalized before returning it!
                //      And the returned value should be a float2 (not a float4!)
                //
                float4 p1 = HermiteMult(controlMatrix, splineMatrix, float4(t*t*t, t*t, t, 1));
                float t2 = t + dt;
                float4 p2 = HermiteMult(controlMatrix, splineMatrix, float4(t2*t2*t2, t2*t2, t2, 1));
                float2 normal = float2((p1.y - p2.y), (p2.x - p1.x));
                return normalize(normal);
            }  // end of GetNormalToCurve()
            


            // ---------------------------------------------------------
            // the Vertex Shader outputs one vertex position:
            //   either
            //     vertex position on the Spline Curve (if offset is 0, i.e. base curve)
            //   or
            //     vertex position on the Offset Curve (if offset is > 0, i.e. offset curve)
            //
            v2f vert (appdata v) {
            
                // the output to this shader is:
                v2f o;
                
                // in the vertex shader's input parameter v,
                //   we receive t in the x component,
                //   and the offset in the y component:
                float t = v.vertex.x;
                float offset = v.vertex.y;
                
                // compute "t to the power of 3,2,1,0":
                float4 tRow = float4(t*t*t, t*t, t, 1);

                //
                // compute matrix of four Control Points for spline:
                //
                float4x4 controlMatrix = transpose(float4x4(
                    float4(_Control0, 0, 0), 
                    float4(_Control1, 0, 0),
                    float4(_Control2, 0, 0), 
                    float4(_Control3, 0, 0)
                ));

                // call HermiteMult to perform the spline equation calculation,
                // to obtain the position of the point on the spline "base" curve:
                float4 worldPosition = HermiteMult(controlMatrix, _SplineMatrix, tRow);

                // calculate the normal, used for the equivalent point on offset curve:
                float2 normal = GetNormalToCurve(t, _Step, controlMatrix, _SplineMatrix);

                // the following will do nothing when offset is 0,
                //         i.e. vertex on "base curve",
                // but it will move the vertex in the direction of the normal
                //         for the vertex on "offset curve", when offset is > 0 :
                worldPosition.xy += normal * offset;

                worldPosition.z = 0;
                worldPosition.w = 1;

                // note: in the last matrix multiplication,
                //  UNITY_MATRIX_VP is the matrix (provided by Unity)
                //  that will transform any vertex
                //    from   world coordinates
                //    to     camera coordinates,
                //    to   NDC / clip coordinates
                // for more information about UNITY_MATRIX_VP, see:
                // https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
                o.vertex = mul(UNITY_MATRIX_VP, worldPosition);
                return o;

                // for the curious: UNITY_MATRIX_VP in the shader
                //    is also equivalent to the following multiplication
                //    in Unity C# scripts:
                //      GL.GetGPUProjectionMatrix(camera.projectionMatrix, ...)
                //      * camera.worldToCameraMatrix

            } // end of vert shader


            // -------------------------------------------------
            // the Fragment Shader simply outputs a fixed color:

            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target {
                return _Color;
            } // end of frag shader

            ENDCG
        }
    }
}
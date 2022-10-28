using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS03 {

    public class SplineDerivatives : MonoBehaviour
    {

        [SerializeField] private Transform control0, control1, control2, control3;
        [SerializeField] private bool normalizeDerivatives;
        [SerializeField] private LineRenderer startFirstDerivative, startSecondDerivative, endFirstDerivative, endSecondDerivative;

        public void ToggleNormalizeDerivatives()
        {
            normalizeDerivatives = !normalizeDerivatives;
        }

        // Update is called once per frame
        public void Update()
        {
            // Call the control points by the x0, x1, etc convention for the sake of ease
            Vector3 x0, x1, x2, x3;
            x0 = control0.position;
            x1 = control1.position;
            x2 = control2.position;
            x3 = control3.position;

            // Calculate 1st and 2nd derivative vectors
            Vector3 d1_t0 = Vector3.zero; // 1st derivative at t=0
            Vector3 d2_t0 = Vector3.zero; // 2nd derivative at t=0
            Vector3 d1_t1 = Vector3.zero; // 1st derivative at t=1
            Vector3 d2_t1 = Vector3.zero; // 2nd derivative at t=1

            switch (SplineSegmentGPUCompute.splineType)
            {
                case SplineParameters.SplineType.Bezier:
                    d1_t0 = 3*(x1 - x0);
                    d2_t0 = 6*(x0 - (2*x1) + x2);
                    d1_t1 = 3*(x3 - x2);
                    d2_t1 = 6*(x1 - (2*x2) + x3);
                    break;

                case SplineParameters.SplineType.CatmullRom:
                    d1_t0 = 0.5f*(x2 - x0);
                    d2_t0 = 2*x0 - 5*x1 + 4*x2 - x3;
                    d1_t1 = 0.5f*(x3 - x1);
                    d2_t1 = -1*x0 + 4*x1 - 5*x2 + 2*x3;
                    break;

                case SplineParameters.SplineType.Bspline:
                    d1_t0 = 0.5f*(x2 - x0);
                    d2_t0 = x0 - 2*x1 + x2;
                    d1_t1 = 0.5f*(x3 - x1);
                    d2_t1 = x1 - 2*x2 + x3;
                    break;
            }

            // We might want to normalize the lengths so they fit on the screen better.
            if (normalizeDerivatives)
            {
                d1_t0 = d1_t0.normalized;
                d2_t0 = d2_t0.normalized;
                d1_t1 = d1_t1.normalized;
                d2_t1 = d2_t1.normalized;
            }

            // Calculate the start and end points of the spline
            Vector3 splineStart = Vector3.zero;
            Vector3 splineEnd = Vector3.zero;

            switch (SplineSegmentGPUCompute.splineType)
            {
                case SplineParameters.SplineType.Bezier:
                    splineStart = x0;
                    splineEnd = x3;
                    break;

                case SplineParameters.SplineType.CatmullRom:
                    splineStart = x1;
                    splineEnd = x2;
                    break;

                case SplineParameters.SplineType.Bspline:
                    splineStart = (x0 + 4*x1 + x2) / 6;
                    splineEnd = (x1 + 4*x2 + x3) / 6;
                    break;
            }

            // Set the points of the line renderers for the derivatives
            // We can discard the z and w values.
            startFirstDerivative.SetPosition(0, splineStart);
            startFirstDerivative.SetPosition(1, splineStart + d1_t0);
            startSecondDerivative.SetPosition(0, splineStart);
            startSecondDerivative.SetPosition(1, splineStart + d2_t0);

            endFirstDerivative.SetPosition(0, splineEnd);
            endFirstDerivative.SetPosition(1, splineEnd + d1_t1);
            endSecondDerivative.SetPosition(0, splineEnd);
            endSecondDerivative.SetPosition(1, splineEnd + d2_t1);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS01 {

    public class PolygonLogic : MonoBehaviour
    {
        [SerializeField] private Transform[] points;
        [SerializeField] private Transform subjectPoint;
        [SerializeField] private LineRenderer connectingLineRenderer;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < points.Length; i++) {
                GameObject vertex = points[i].gameObject;
                // Each vertex is responsible for rendering a line segment between it and its next neighbor
                
                LineRenderer lr = vertex.AddComponent<LineRenderer>() as LineRenderer;
                lr.startWidth = 0.1f;
                lr.endWidth = 0.1f;
                lr.startColor = new Color(0,1,0,1);
                lr.endColor = new Color(0,1,0,1);

                // Thanks to https://stackoverflow.com/questions/72240485/how-to-add-the-default-line-material-back-to-the-linerenderer-material
                // for this solution
                lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            }
        }

        // Update is called once per frame
        void Update()
        {
            // This loop connects all the vertices
            for (int i = 0; i < points.Length; i++) {
                LineRenderer lr = (points[i].gameObject).GetComponent<LineRenderer>();

                if (lr != null) {
                    lr.SetPosition(0, points[i].position);
                    lr.SetPosition(1, points[(i+1) % points.Length].position);
                }
                else {
                    Debug.Log("Vertex " + i + " has no LineRenderer component!");
                }
            }

            // This connects the subject point to the closest point on the Polygon
            connectingLineRenderer.SetPosition(0, subjectPoint.position);
            connectingLineRenderer.SetPosition(1, LineUtility.ClosestPointOnPolygon(points, subjectPoint.position));
        }
    }

}
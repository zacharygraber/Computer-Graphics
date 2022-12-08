using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class AxisLogic : MonoBehaviour
    {
        [SerializeField] public Material material;
        [SerializeField] public CustomCamera cam;
        [SerializeField, Range(0.0f, 0.05f)] public float thickness;
        [SerializeField] public float tickHeight = 0.5f;
        [SerializeField] public float tickThickness = 0.03f;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh mesh;

        // Start is called before the first frame update
        void Start()
        {
            // Obtain Mesh Renderer and Mesh Filter components from Unity scene and generate a cube mesh:
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            mesh = new Mesh();

            Vector3[] verts = new Vector3[192];
            Vector3[] normals = new Vector3[192];
            int[] tris = new int[44 * 3 * 3]; // 20 ticks per axis * 2 tris per tick * 3 axes (plus tris for axes themselves)

            ///////////////////////////////
            // Create the main axis lines /
            ///////////////////////////////

            // z-axis
            verts[0] = new Vector3(-1*thickness, 0, 0);
            verts[1] = new Vector3(thickness, 0, 0);
            verts[2] = new Vector3(thickness, 0, 10);
            verts[3] = new Vector3(-1*thickness, 0, 10);
            tris[0] = 0; tris[1] = 1; tris[2] = 2;
            tris[3] = 2; tris[4] = 3; tris[5] = 0;
            tris[6] = 0; tris[7] = 3; tris[8] = 2;
            tris[9] = 2; tris[10] = 1; tris[11] = 0;

            // x-axis
            verts[64] = new Vector3(0, 0, -1*thickness);
            verts[65] = new Vector3(0, 0, thickness);
            verts[66] = new Vector3(10, 0, thickness);
            verts[67] = new Vector3(10, 0, -1*thickness);
            tris[132] = 64; tris[133] = 65; tris[134] = 66;
            tris[135] = 66; tris[136] = 67; tris[137] = 64;
            tris[138] = 67; tris[139] = 66; tris[140] = 65;
            tris[141] = 65; tris[142] = 64; tris[143] = 67;

            // y-axis
            verts[128] = new Vector3(-1*thickness, 0, 0);
            verts[129] = new Vector3(thickness, 0, 0);
            verts[130] = new Vector3(thickness, 10, 0);
            verts[131] = new Vector3(-1*thickness, 10, 0);
            tris[264] = 131; tris[265] = 130; tris[266] = 129;
            tris[267] = 129; tris[268] = 128; tris[269] = 131;
            tris[270] = 128; tris[271] = 129; tris[272] = 130;
            tris[273] = 130; tris[274] = 131; tris[275] = 128;

            /////////////////////////////////////////
            // Create the ticks at each unit length /
            /////////////////////////////////////////

            // z-axis
            int tickOffset, triOffset;
            for (int i = 0; i < 10; i++) {
                tickOffset = 4 + (3 * i);
                triOffset = 12 + (6 * i);
                verts[tickOffset] = new Vector3(0, 0, i - tickThickness + 1);
                verts[tickOffset + 1] = new Vector3(0, 0, i + tickThickness + 1);
                verts[tickOffset + 2] = new Vector3(0, tickHeight, i + 1);
                tris[triOffset] = tickOffset; tris[triOffset+1] = tickOffset+1; tris[triOffset+2] = tickOffset+2;
                tris[triOffset+3] = tickOffset+2; tris[triOffset+4] = tickOffset+1; tris[triOffset+5] = tickOffset;
            }
            for (int i = 0; i < 10; i++) {
                tickOffset = 34 + (3 * i);
                triOffset = 72 + (6 * i);
                verts[tickOffset] = new Vector3(0, 0, i - tickThickness + 1);
                verts[tickOffset + 1] = new Vector3(0, 0, i + tickThickness + 1);
                verts[tickOffset + 2] = new Vector3(tickHeight, 0, i + 1);
                tris[triOffset] = tickOffset; tris[triOffset+1] = tickOffset+1; tris[triOffset+2] = tickOffset+2;
                tris[triOffset+3] = tickOffset+2; tris[triOffset+4] = tickOffset+1; tris[triOffset+5] = tickOffset;
            }

            // x-axis
            for (int i = 0; i < 10; i++) {
                tickOffset = 68 + (3 * i);
                triOffset = 144 + (6 * i);
                verts[tickOffset] = new Vector3(i - tickThickness + 1, 0, 0);
                verts[tickOffset + 1] = new Vector3(i + tickThickness + 1, 0, 0);
                verts[tickOffset + 2] = new Vector3(i + 1, tickHeight, 0);
                tris[triOffset] = tickOffset; tris[triOffset+1] = tickOffset+1; tris[triOffset+2] = tickOffset+2;
                tris[triOffset+3] = tickOffset+2; tris[triOffset+4] = tickOffset+1; tris[triOffset+5] = tickOffset;
            }
            for (int i = 0; i < 10; i++) {
                tickOffset = 98 + (3 * i);
                triOffset = 204 + (6 * i);
                verts[tickOffset] = new Vector3(i - tickThickness + 1, 0, 0);
                verts[tickOffset + 1] = new Vector3(i + tickThickness + 1, 0, 0);
                verts[tickOffset + 2] = new Vector3(i + 1, 0, tickHeight);
                tris[triOffset] = tickOffset; tris[triOffset+1] = tickOffset+1; tris[triOffset+2] = tickOffset+2;
                tris[triOffset+3] = tickOffset+2; tris[triOffset+4] = tickOffset+1; tris[triOffset+5] = tickOffset;
            }

            // y-axis
            for (int i = 0; i < 10; i++) {
                tickOffset = 132 + (3 * i);
                triOffset = 276 + (6 * i);
                verts[tickOffset] = new Vector3(0, i - tickThickness + 1, 0);
                verts[tickOffset + 1] = new Vector3(0, i + tickThickness + 1, 0);
                verts[tickOffset + 2] = new Vector3(tickHeight, i + 1, 0);
                tris[triOffset] = tickOffset; tris[triOffset+1] = tickOffset+1; tris[triOffset+2] = tickOffset+2;
                tris[triOffset+3] = tickOffset+2; tris[triOffset+4] = tickOffset+1; tris[triOffset+5] = tickOffset;
            }
            for (int i = 0; i < 10; i++) {
                tickOffset = 162 + (3 * i);
                triOffset = 336 + (6 * i);
                verts[tickOffset] = new Vector3(0, i - tickThickness + 1, 0);
                verts[tickOffset + 1] = new Vector3(0, i + tickThickness + 1, 0);
                verts[tickOffset + 2] = new Vector3(0, i + 1, tickHeight);
                tris[triOffset] = tickOffset; tris[triOffset+1] = tickOffset+1; tris[triOffset+2] = tickOffset+2;
                tris[triOffset+3] = tickOffset+2; tris[triOffset+4] = tickOffset+1; tris[triOffset+5] = tickOffset;
            }

            // Set normals to color the axes (in the shader, determined by normal direction x=r, y=g, z=b)
            for (int i = 0; i < 192; i++) {
                if (i < 64) {
                    normals[i] = Vector3.forward; // z-axis
                }
                else if (i < 128) {
                    normals[i] = Vector3.right; // x-axis
                }
                else {
                    normals[i] = Vector3.up; // y-axis
                }
            }
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.normals = normals;

            meshFilter.mesh = mesh;

            // Assign our custom material/shader to the mesh
            meshRenderer.material = material;

            // initialize modeling matrix (no tranformations need to be done)
            material.SetMatrix("_TranslationMatrix", Matrix4x4.identity);
            material.SetMatrix("_RotationMatrix", Matrix4x4.identity);
            material.SetMatrix("_ScalingMatrix", Matrix4x4.identity);

            // initialize viewing matrix
            material.SetMatrix("_ViewingMatrix", cam.viewMatrix);

            // initialize projection matrix
            material.SetMatrix("_ProjectionMatrix", cam.projectionMatrix);
        }

        // Update is called once per frame
        void Update()
        {
            // Compute viewing transformation matrix
            material.SetMatrix("_ViewingMatrix", cam.viewMatrix);

            // Compute projection matrix
            material.SetMatrix("_ProjectionMatrix", cam.projectionMatrix);
        }
    }
}
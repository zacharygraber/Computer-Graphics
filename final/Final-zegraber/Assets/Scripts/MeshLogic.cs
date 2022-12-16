using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05
{
    public class MeshLogic : MonoBehaviour
    {
        [Header("Scene Elements")]
        [SerializeField] public Shader shader;
        [SerializeField] private CustomCamera cam;
        [SerializeField] private CustomTransform _customTransform;
        [SerializeField] private CustomLight _light;
        public CustomLight[] lights;

        [Header("Material Properties")]
        [SerializeField, Range(1.0f, 1000.0f)] private float specularity;
        [SerializeField, Range(0.0f, 2.0f)] private float diffuseIntensity;
        [SerializeField, Range(0.0f, 1.0f)] private float ambientIntensity;
        [SerializeField] private Color surfaceColor;

        protected MeshFilter meshFilter;
        protected MeshRenderer meshRenderer;
        protected Mesh mesh;
        protected Material material;

        private Color[] lightDiffColors;
        private Color[] lightSpecularColors;
        private Vector4[] lightPositions;
        private float[] lightPowers;
        private int numLights;

        void Awake()
        {
            lights = (CustomLight[]) GameObject.FindObjectsOfType(typeof(CustomLight));
            numLights = lights.Length;
            lightDiffColors = new Color[numLights];
            lightSpecularColors = new Color[numLights];
            lightPositions = new Vector4[numLights];
            lightPowers = new float[numLights];
        }

        // Update is called once per frame
        void Update()
        {
            // Compute and pass the modeling matrices to the shader
            SetModelingMatrices();
            
            // Compute viewing transformation matrix
            material.SetMatrix("_ViewingMatrix", cam.viewMatrix);

            // Compute projection matrix
            material.SetMatrix("_ProjectionMatrix", cam.projectionMatrix);

            // Set lighting-related uniforms
            // Material properties
            material.SetVector("_CameraPos", cam.getPosition());
            material.SetVector("_MaterialColor", surfaceColor);
            material.SetFloat("_LightAmbient", ambientIntensity);
            material.SetFloat("_Specularity", specularity);
            material.SetFloat("_DiffuseIntensity", diffuseIntensity);

            // Light Property arrays
            for (int i = 0; i < numLights; i++) {
                lightDiffColors[i] = lights[i].diffuseColor;
                lightSpecularColors[i] = lights[i].specularColor;
                Vector3 pos3 = lights[i].getPosition();
                lightPositions[i] = new Vector4(pos3.x, pos3.y, pos3.z, 1.0f);
                lightPowers[i] = lights[i].lightPower;
            }
            material.SetVectorArray("_LightPos", lightPositions);
            material.SetColorArray("_LightDiffuseColor", lightDiffColors);
            material.SetColorArray("_LightSpecularColor", lightSpecularColors);
            material.SetFloatArray("_LightPower", lightPowers);
            material.SetInt("_NumLights", numLights);
        }

        public virtual void SetMesh(Mesh newMesh) {
            this.mesh = newMesh;
            this.mesh.RecalculateNormals();

            this.meshRenderer = GetComponent<MeshRenderer>();
            this.meshFilter = GetComponent<MeshFilter>();
            this.meshFilter.mesh = this.mesh;

            this.material = new Material(this.shader);
            this.meshRenderer.material = this.material;
        }

        public Material getMaterial() {
            return this.material;
        }

        private void SetModelingMatrices() {
            material.SetMatrix("_TranslationMatrix", new Matrix4x4(
                new Vector4(1,0,0,0),
                new Vector4(0,1,0,0),
                new Vector4(0,0,1,0),
                new Vector4(_customTransform.position.x, _customTransform.position.y, _customTransform.position.z,1)
            ));
            material.SetMatrix("_ScalingMatrix", new Matrix4x4(
                new Vector4(_customTransform.scale.x == 0 ? 0.001f : _customTransform.scale.x,0,0,0), // ensure we never scale by 0
                new Vector4(0,_customTransform.scale.y == 0 ? 0.001f : _customTransform.scale.y,0,0),
                new Vector4(0,0,_customTransform.scale.z == 0 ? 0.001f : _customTransform.scale.z,0), 
                new Vector4(0,0,0,1)
            ));
            material.SetMatrix("_RotationMatrix", _customTransform.GetRotationMatrix());
        }

        // public static Mesh generateDodecahedronMesh() {
        //     Vector3[] verts = new Vector3[20];
        //     int[] tris = new int[12*3*3]; // 12 pentagons * 3 tris/pentagon * 3 indices per tri

        //     // Basically counting in binary on a 3-bit number for alternating signs of x,y,z
        //     // where -1=false and 1=true
        //     int[] sign = {-1,-1,-1};
        //     int offset = 0;
        //     while (true) {


        //         // flip signs around
        //         if (sign[0] == -1) sign[0] = 1;
        //         else if (sign[1] == -1) {sign[1] = 1; sign[0] = -1;}
        //         else if (sign[2] == -1) {sign[2] = 1; sign[1] = -1; sign[0] = -1;}
        //         else {break;}

        //         offset += 4;
        //     }
        // }

        // Helper function to get a cube. Used for point lights, but mostly for debug/fun
        public static Mesh GetCubeMesh() {
            Mesh mesh = new Mesh();

            Vector3[] vertices = { // Repeat vertices are necessary for proper normals
                new Vector3(-.5f, .5f, -.5f), // top face
                new Vector3(-.5f, .5f, .5f), // top face
                new Vector3(.5f, .5f, .5f), // top face 
                new Vector3(.5f, .5f, -.5f), // top face
                new Vector3(-.5f, -.5f, -.5f), // bottom face
                new Vector3(-.5f, -.5f, .5f), // bottom face
                new Vector3(.5f, -.5f, .5f), // bottom face
                new Vector3(.5f, -.5f, -.5f), // bottom face
                new Vector3(-.5f, .5f, -.5f), // left face
                new Vector3(-.5f, -.5f, -.5f), // left face
                new Vector3(-.5f, -.5f, .5f), // left face
                new Vector3(-.5f, .5f, .5f), // left face
                new Vector3(.5f, .5f, -.5f), // right face
                new Vector3(.5f, .5f, .5f), // right face
                new Vector3(.5f, -.5f, .5f), // right face
                new Vector3(.5f, -.5f, -.5f), // right face
                new Vector3(-.5f, .5f, -.5f), // front face
                new Vector3(.5f, .5f, -.5f), // front face
                new Vector3(.5f, -.5f, -.5f), // front face
                new Vector3(-.5f, -.5f, -.5f), // front face
                new Vector3(-.5f, .5f, .5f), // back face
                new Vector3(-.5f, -.5f, .5f), // back face
                new Vector3(.5f, -.5f, .5f), // back face
                new Vector3(.5f, .5f, .5f) // back face
            };

            // Vector3[] normals = {
            //     Vector3.up, Vector3.up, Vector3.up, Vector3.up, // top
            //     Vector3.down, Vector3.down, Vector3.down, Vector3.down, // bottom
            //     Vector3.left, Vector3.left, Vector3.left, Vector3.left, // left
            //     Vector3.right, Vector3.right, Vector3.right, Vector3.right, // right
            //     Vector3.back, Vector3.back, Vector3.back, Vector3.back, // front
            //     Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, // back
            // };
            // Vector3[] normals = {
            //     Vector3.down, Vector3.down, Vector3.down, Vector3.down, // bottom
            //     Vector3.up, Vector3.up, Vector3.up, Vector3.up, // top
            //     Vector3.right, Vector3.right, Vector3.right, Vector3.right, // right
            //     Vector3.left, Vector3.left, Vector3.left, Vector3.left, // left
            //     Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, // back
            //     Vector3.back, Vector3.back, Vector3.back, Vector3.back // front
            // };

            int[] triangles = {
                2,  1,  0, // top
                0,  3,  2, // top
                5,  6,  7, // bottom
                7,  4,  5, // bottom
                10,  9,  8, // left
                8, 11, 10, // left
                14, 13, 12, // right
                12, 15, 14, // right
                18, 17, 16, // front
                16, 19, 18, // front
                22, 21, 20, // back
                20, 23, 22 // back
            };
            // int[] triangles = {
            //     0,  1,  2, // top
            //     2,  3,  0, // top
            //     7,  6,  5, // bottom
            //     5,  4,  7, // bottom
            //     8,  9,  10, // left
            //     10, 11, 8, // left
            //     12, 13, 14, // right
            //     14, 15, 12, // right
            //     16, 17, 18, // front
            //     18, 19, 16, // front
            //     20, 21, 22, // back
            //     22, 23, 20 // back
            // };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            // mesh.normals = normals;

            return mesh;
        }
    }
}
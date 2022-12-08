using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05
{
    public class MeshLogic : MonoBehaviour
    {
        [SerializeField] public Material material;
        [SerializeField] public CustomCamera cam;
        [SerializeField] public CustomTransform _customTransform;
        [SerializeField] private CustomLight _light;

        [SerializeField, Range(0.0f, 10.0f)] private float specularity;
        [SerializeField, Range(0.0f, 1.0f)] private float diffuseIntensity;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh mesh;

        // Start is called before the first frame update
        void Start()
        {
            // Obtain Mesh Renderer and Mesh Filter components from Unity scene and generate a cube mesh:
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            mesh = GetCubeMesh();
            meshFilter.mesh = mesh;

            // Assign our custom material/shader to the mesh
            meshRenderer.material = material;
        }

        // Update is called once per frame
        void Update()
        {
            // Compute and pass the modeling matrices to the shader
            SetModelingMatrices(material);
            
            // Compute viewing transformation matrix
            material.SetMatrix("_ViewingMatrix", cam.viewMatrix);

            // Compute projection matrix
            material.SetMatrix("_ProjectionMatrix", cam.projectionMatrix);

            // Set lighting-related uniforms
            material.SetVector("_CameraPos", cam.getPosition());
            material.SetVector("_LightPos", _light.getPosition());
            material.SetVector("_LightDiffuseColor", _light.diffuseColor);
            material.SetVector("_LightSpecularColor", _light.specularColor);
            material.SetFloat("_LightAmbient", _light.ambientIntensity);
            material.SetFloat("_LightIntensity", _light.intensityMultiplier);
            material.SetFloat("_Specularity", specularity);
            material.SetFloat("_DiffuseIntensity", diffuseIntensity);
        }

        

        private void SetModelingMatrices(Material material) {
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

        // Helper function to clear up Start() function above
        private Mesh GetCubeMesh() {
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

            Vector3[] normals = {
                Vector3.up, Vector3.up, Vector3.up, Vector3.up, // top
                Vector3.down, Vector3.down, Vector3.down, Vector3.down, // bottom
                Vector3.left, Vector3.left, Vector3.left, Vector3.left, // left
                Vector3.right, Vector3.right, Vector3.right, Vector3.right, // right
                Vector3.back, Vector3.back, Vector3.back, Vector3.back, // front
                Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, // back
            };
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
            mesh.normals = normals;

            return mesh;
        }
    }
}
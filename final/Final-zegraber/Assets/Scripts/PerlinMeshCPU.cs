using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class PerlinMeshCPU : MonoBehaviour
    {
        [SerializeField] public MeshLogic meshLogic;

        [Header("Mesh Parameters")]
        [SerializeField, Range(4, 255)] private int meshSize; // Square mesh with `meshSize` by `meshSize` sub-squares
        [SerializeField, Range(0.0f, 10.0f)] private float heightScale = 1.0f;

        [Header("Perlin Noise Parameters")]
        [SerializeField, Range(0.0f, 0.999f)] private float step;
        [SerializeField] private Vector2 offset;

        private Mesh mesh;
        private Texture2D heightMap;

        // Track changes so we can update in real-time
        private float prevStep, prevHeightScale;
        private Vector2 prevOffset;
        private int prevMeshSize;

        // Start is called before the first frame update
        void Start()
        {
            mesh = generateFlatMesh(this.meshSize);
            perlinizeMesh();

            meshLogic.SetMesh(mesh);

            prevStep = step;
            prevHeightScale = heightScale;
            prevOffset = offset;
            prevMeshSize = meshSize;
        }

        // Update is called once per frame
        void Update()
        {
            if (step != prevStep || heightScale != prevHeightScale || offset != prevOffset) {
                perlinizeMesh();
                meshLogic.SetMesh(mesh);
            }
            else if (meshSize != prevMeshSize) {
                mesh = generateFlatMesh(this.meshSize);
                perlinizeMesh();
                meshLogic.SetMesh(mesh);
            }
            prevStep = step;
            prevHeightScale = heightScale;
            prevOffset = offset;
            prevMeshSize = meshSize;
        }

        private void perlinizeMesh() {
            int nVert = this.meshSize+1;
            float[,] heightMap = generatePerlinHeightLookup(nVert, this.step, this.offset); // Computed CPU-side for now, but could easily be moved to a vertex shader
            int rowOffset;
            List<Vector3> verts = new List<Vector3>();
            this.mesh.GetVertices(verts);
            Vector3 thisVert;
            for (int row = 0; row < nVert; row++) {
                rowOffset = row * nVert;
                for (int col = 0; col < nVert; col++) {
                    thisVert = verts[rowOffset+col];
                    thisVert.y = (heightMap[row, col] * heightScale * 2) - heightScale;
                    verts[rowOffset+col] = thisVert;
                }
            }
            this.mesh.SetVertices(verts);
        }

        private static Mesh generateFlatMesh(int meshSize) {
            // Generate the base "flat" mesh vertices
            Mesh m = new Mesh();
            int nVert = meshSize + 1; // number of verts in a row/col--for convenience
            Vector3[] verts = new Vector3[nVert*nVert]; // arranged [r0c1, r0c2, r0c3, r1c0, r1c2, ...] for 3x3 verts
            Vector2[] uvs = new Vector2[verts.Length];
            int rowOffset;
            for (int row = 0; row < nVert; row++) {
                rowOffset = row * nVert;
                for (int col = 0; col < nVert; col++) {
                    Vector3 vertPos = new Vector3(-1.0f + (2.0f / meshSize)*row, 0, -1.0f + (2.0f / meshSize)*col);
                    verts[rowOffset + col] = vertPos;
                    uvs[rowOffset + col] = new Vector2((vertPos.x * 0.5f) + 0.5f, (vertPos.z * 0.5f) + 0.5f);
                }
            }

            // Set up mesh triangles
            int[] tris = new int[2 * meshSize * meshSize * 3];
            int trisOffset = 0;
            for (int row = 0; row < meshSize; row++) {
                rowOffset = row * nVert;
                for (int col = 0; col < meshSize; col++) {
                    // 6 tris indices for each iteration of this loop
                    // Triangle 1 for this square
                    tris[trisOffset] = rowOffset+col;
                    tris[trisOffset+1] = (rowOffset+nVert)+col;
                    tris[trisOffset+2] = (rowOffset+nVert)+col+1;

                    // Triangle 2
                    tris[trisOffset+3] = (rowOffset+nVert)+col+1;
                    tris[trisOffset+4] = rowOffset+col+1;
                    tris[trisOffset+5] = rowOffset+col;

                    // Increment offset for next iter
                    trisOffset += 6;
                }
            }
            
            m.vertices = verts;
            m.triangles = tris;
            m.uv = uvs;
            return m;
        }

        // Gives a 2D array of heights unscaled, ranged [0.0, 1.0]
        private static float[,] generatePerlinHeightLookup(int nVerts, float step, Vector2 offset) {
            float[,] heightMap = new float[nVerts, nVerts];
            for (int i = 0; i < nVerts; i++) {
                for (int j = 0; j < nVerts; j++) {
                    heightMap[i, j] = Mathf.PerlinNoise(offset.x + (step*i), offset.y + (step*j));
                }
            }
            return heightMap;
        }
    }
}
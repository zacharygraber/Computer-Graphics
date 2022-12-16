using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class PerlinMesh : MonoBehaviour
    {
        [SerializeField] public MeshLogic meshLogic;

        [Header("Mesh Parameters")]
        [SerializeField, Range(4, 255)] private int meshSize; // Square mesh with `meshSize` by `meshSize` sub-squares
        [SerializeField, Range(0.0f, 10.0f)] public float heightScale = 1.0f;

        [Header("Perlin Noise Parameters")]
        [SerializeField, Range(0.0f, 0.999f)] private float perlinScale;
        [SerializeField] public Vector2 offset;
        [SerializeField] private int heightMapResolution = 4096;
        [SerializeField] private float noiseSampleSize;

        private Mesh mesh;
        private Texture2D heightMap;

        // Track changes so we can update in real-time
        private int prevMeshSize, prevHeightMapRes;
        private float prevSampleSize;

        // Start is called before the first frame update
        void Start()
        {
            mesh = generateFlatMesh(this.meshSize);
            heightMap = generatePerlinTexture();

            meshLogic.SetMesh(mesh);

            prevMeshSize = meshSize;
            prevHeightMapRes = heightMapResolution;
            prevSampleSize = noiseSampleSize;
        }

        // Update is called once per frame
        void Update()
        {
            if (meshSize != prevMeshSize || noiseSampleSize != prevSampleSize) {
                mesh = generateFlatMesh(this.meshSize);
                meshLogic.SetMesh(mesh);
            }
            if (heightMapResolution != prevHeightMapRes) {
                heightMap = generatePerlinTexture();
            }
            prevMeshSize = meshSize;
            prevHeightMapRes = heightMapResolution;
            prevSampleSize = noiseSampleSize;

            meshLogic.getMaterial().SetFloat("_HeightScale", heightScale);
            meshLogic.getMaterial().SetTexture("_AlphaHeightMap", heightMap);
            meshLogic.getMaterial().SetVector("_PerlinOffset", offset);
            meshLogic.getMaterial().SetFloat("_PerlinScale", perlinScale);
            meshLogic.getMaterial().SetTexture("_MainTex", ((PerlinMeshLogic) meshLogic).texture);
        }

        public static Mesh generateFlatMesh(int meshSize) {
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

        private Texture2D generatePerlinTexture() {
            Texture2D tex = new Texture2D(heightMapResolution, heightMapResolution);

            float step = noiseSampleSize/heightMapResolution;
            
            for (int i = 0; i < heightMapResolution; i++) {
                for (int j = 0; j < heightMapResolution; j++) {
                    float pVal = Mathf.PerlinNoise(i*step, j*step);
                    tex.SetPixel(i, j, new Color(0.0f, 0.0f, 0.0f, pVal));
                }
            }
            tex.Apply();
            return tex;
        }
    }
}
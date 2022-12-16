using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class AnimatePerlinWater : MonoBehaviour
    {
        [SerializeField] private PerlinMesh perlinMesh;

        [SerializeField] private bool scrollOffset, oscillateHeightScale;
        [SerializeField, Range(0.0f, 0.0001f)] private float xScrollSpeed, yScrollSpeed;
        [SerializeField, Range(0.0f, 0.1f)] private float heightMin, heightMax;
        [SerializeField, Range(0, 0.1f)] private float heightOscillationSpeed;

        private enum direction {
            DOWN,
            UP
        }

        private direction currDir;

        // Start is called before the first frame update
        void Start()
        {
            if (oscillateHeightScale) {
                perlinMesh.heightScale = heightMin;
            }
            currDir = direction.UP;
        }

        // Update is called once per frame
        void Update()
        {
            if (scrollOffset) {
                perlinMesh.offset += (new Vector2(xScrollSpeed, yScrollSpeed));
            }
            if (oscillateHeightScale) {
                if (currDir == direction.UP) {
                    perlinMesh.heightScale = Mathf.Min(heightMax, perlinMesh.heightScale + heightOscillationSpeed);
                }
                else {
                    perlinMesh.heightScale = Mathf.Max(heightMin, perlinMesh.heightScale - heightOscillationSpeed);
                }

                if (perlinMesh.heightScale >= heightMax) {
                    currDir = direction.DOWN;
                }
                else if (perlinMesh.heightScale <= heightMin) {
                    currDir = direction.UP;
                }
            }
        }
    }
}
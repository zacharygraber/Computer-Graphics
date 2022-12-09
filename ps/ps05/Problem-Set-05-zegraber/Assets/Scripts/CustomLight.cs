using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class CustomLight : MonoBehaviour
    {
        [SerializeField] private CustomTransform _customTransform;

        [SerializeField] public Color diffuseColor, specularColor;
        [SerializeField, Range(0.0f, 1.0f)] public float ambientIntensity;
        [SerializeField, Range(0.0f, 3.0f)] public float intensityMultiplier;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private CustomCamera cam;
        [SerializeField] private bool animate;
        [SerializeField, Range(1.0f, 20.0f)] private float orbitRadius;
        [SerializeField, Range(0.0f, 10.0f)] private float animationSpeed;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            mesh = MeshLogic.GetCubeMesh();
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
        }

        void Update()
        {
            if (animate) {
                _customTransform.position.x = orbitRadius * Mathf.Cos(Time.time * animationSpeed);
                _customTransform.position.z = orbitRadius * Mathf.Sin(Time.time * animationSpeed);
            }

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

            // Compute viewing transformation matrix
            material.SetMatrix("_ViewingMatrix", cam.viewMatrix);

            // Compute projection matrix
            material.SetMatrix("_ProjectionMatrix", cam.projectionMatrix);
            material.SetVector("_PointLightColor", diffuseColor);
        }

        public Vector3 getPosition() {
            return this._customTransform.position;
        }
    }
}
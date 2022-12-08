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

        public Vector3 getPosition() {
            return this._customTransform.position;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS04 {
    public class CustomCamera : MonoBehaviour
    {
        [Header ("Camera Options")]
        [SerializeField] private CustomTransform _customTransform;
        [SerializeField, Range(1, 180)] private int fovY = 70;
        [SerializeField] private float aspectX = 16f, aspectY = 9f;
        [SerializeField] private float near = 0.01f, far = 100.0f;

        public Matrix4x4 viewMatrix;
        public Matrix4x4 projectionMatrix;

        // Update is called once per frame
        void Update()
        {
            viewMatrix = ComputeViewingMatrix();
            projectionMatrix = ComputeCameraMatrix();
        }

        private Matrix4x4 ComputeViewingMatrix() {
            Matrix4x4 transform = new Matrix4x4(
                new Vector4(1,0,0,0),
                new Vector4(0,1,0,0),
                new Vector4(0,0,1,0),
                new Vector4(-1*_customTransform.position.x, -1*_customTransform.position.y, -1*_customTransform.position.z,1)
            );
            // TODO: FIX backwards rotation
            return _customTransform.GetRotationMatrix() * transform;
        }

        // Basically an implementation of gluPerspective
        private Matrix4x4 ComputeCameraMatrix() {
            return new Matrix4x4(
                new Vector4(1 / ((aspectX/aspectY) * Mathf.Tan((fovY / 2) * Mathf.Deg2Rad)),0,0,0),
                new Vector4(0, 1 / (Mathf.Tan((fovY / 2) * Mathf.Deg2Rad)), 0, 0),
                new Vector4(0, 0, -1*((far + near) / (far-near)), -1),
                new Vector4(0, 0, -1*((2*far*near)/(far-near)), 0)
            );
        }
    }
}
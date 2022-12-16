using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class CustomCamera : MonoBehaviour
    {
        [Header ("Camera Options")]
        [SerializeField] public CustomTransform _customTransform;
        [SerializeField, Range(1, 180)] public int fovY = 70;
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
            Matrix4x4 translate = new Matrix4x4(
                new Vector4(1,0,0,0),
                new Vector4(0,1,0,0),
                new Vector4(0,0,1,0),
                new Vector4(-1*_customTransform.position.x, -1*_customTransform.position.y, -1*_customTransform.position.z,1)
            );
            return CustomTransform.ComputeRotationMatrix(-1*_customTransform.rotation) * translate;
        }

        // Basic implementation of gluPerspective
        public Matrix4x4 ComputeCameraMatrix() {
            return new Matrix4x4(
                new Vector4(1 / ((aspectX/aspectY) * Mathf.Tan((fovY / 2) * Mathf.Deg2Rad)),0,0,0),
                new Vector4(0, 1 / (Mathf.Tan((fovY / 2) * Mathf.Deg2Rad)), 0, 0),
                new Vector4(0, 0, -1*((far + near) / (far-near)), -1),
                new Vector4(0, 0, -1*((2*far*near)/(far-near)), 0)
            );
        }

        public Vector3 getPosition() {
            return _customTransform.position;
        }

        private void translateRelativeToCameraVectorXZ(Vector3 direction, float dist) {
            Matrix4x4 R = CustomTransform.ComputeRotationMatrix(_customTransform.rotation);
            Vector4 camDir4 = R * (new Vector4(direction.x, direction.y, direction.z,1));
            Vector3 camDir = ((Vector3) camDir4).normalized;
            _customTransform.Translate(camDir.x * dist, 0, camDir.z * dist);
        }
        public void moveInCameraForwardDirectionXZ(float dist) {
            // the vector pointing in the negative z-direction is the camera's forward
            translateRelativeToCameraVectorXZ(new Vector3(0,0,-1), dist);
        }
        public void moveInCameraLeftDirectionXZ(float dist) {
            translateRelativeToCameraVectorXZ(new Vector3(-1,0,0), dist);
        }
        public void moveInCameraRightDirectionXZ(float dist) {
            translateRelativeToCameraVectorXZ(new Vector3(1,0,0), dist);
        }

        private void translateRelativeToCameraVector(Vector3 direction, float dist) {
            Matrix4x4 R = CustomTransform.ComputeRotationMatrix(_customTransform.rotation);
            Vector4 camDir4 = R * (new Vector4(direction.x, direction.y, direction.z,1));
            Vector3 camDir = ((Vector3) camDir4).normalized;
            _customTransform.Translate(camDir * dist);
        }
        public void moveInCameraForwardDirection(float dist) {
            // the vector pointing in the negative z-direction is the camera's forward
            translateRelativeToCameraVector(new Vector3(0,0,-1), dist);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class CustomTransform : MonoBehaviour
    {
        [Header ("Transform Properties")]
        [SerializeField] public Vector3 position = Vector3.zero;
        // [SerializeField] private Matrix4x4 rotationMatrix = Matrix4x4.identity;
        [SerializeField] public Vector3 rotation;
        [SerializeField] public Vector3 scale = Vector3.one;

        public void Rotate(float x, float y, float z) {
            this.rotation += new Vector3(x, y, z);
        }

        // Prevents the camera from being rotated more than 90 degrees up or down
        public void RotateClampX(float x, float y, float z) {
            Vector3 rotVec = new Vector3(x,y,z);
            Vector3 newRot = this.rotation + rotVec;
            if (newRot.x > 90) {
                newRot.x = 90;
            }
            else if (newRot.x < -90) {
                newRot.x = -90;
            }
            this.rotation = newRot;
        }

        // public void Rotate(Matrix4x4 rotM) {
        //     rotationMatrix = rotM * rotationMatrix;
        // }

        public void Translate(float x, float y, float z) {
            this.position += new Vector3(x, y, z);
        }

        public void Translate(Vector3 delta) {
            Translate(delta.x, delta.y, delta.z);
        }

        public void Scale(float x, float y, float z) {
            this.scale += new Vector3(x, y, z);
        }

        public static Matrix4x4 ComputeRotationMatrix(Vector3 rotVec) {
            return ComputeRotationMatrix(rotVec.x, rotVec.y, rotVec.z);
        }

        // Gets a rotation matrix for rotation parameters
        public static Matrix4x4 ComputeRotationMatrix(float x, float y, float z) {
            // Rotate about x by A, y by B, z by G
            // Defining these shorthands saves sanity and computation.
            // Following matrices at https://en.wikipedia.org/wiki/Rotation_matrix
            float cosA = Mathf.Cos(x * Mathf.Deg2Rad);
            float cosB = Mathf.Cos(y * Mathf.Deg2Rad);
            float cosG = Mathf.Cos(z * Mathf.Deg2Rad);
            float sinA = Mathf.Sin(x * Mathf.Deg2Rad);
            float sinB = Mathf.Sin(y * Mathf.Deg2Rad);
            float sinG = Mathf.Sin(z * Mathf.Deg2Rad);

            Matrix4x4 xRot = new Matrix4x4(
                new Vector4(1,0,0,0),
                new Vector4(0,cosA,-1*sinA,0),
                new Vector4(0,sinA,cosA,0),
                new Vector4(0,0,0,1)
            );
            Matrix4x4 yRot = new Matrix4x4(
                new Vector4(cosB,0,-sinB,0),
                new Vector4(0,1,0,0),
                new Vector4(1*sinB,0,cosB,0),
                new Vector4(0,0,0,1)
            );
            Matrix4x4 zRot = new Matrix4x4(
                new Vector4(cosG,-1*sinG,0,0),
                new Vector4(sinG,cosG,0,0),
                new Vector4(0,0,1,0),
                new Vector4(0,0,0,1)
            );

            return xRot * yRot * zRot;
        }

        public Matrix4x4 GetRotationMatrix() {
            return ComputeRotationMatrix(this.rotation);
        }

        public Vector4 GetPositionAsVector4() {
            return new Vector4(position.x, position.y, position.z, 1);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class CustomTransform : MonoBehaviour
    {
        [Header ("Transform Properties")]
        [SerializeField] public Vector3 position = Vector3.zero;
        [SerializeField] private Matrix4x4 rotationMatrix = Matrix4x4.identity;
        [SerializeField] public Vector3 scale = Vector3.one;

        public void Rotate(float x, float y, float z) {
            rotationMatrix = ComputeRotationMatrix(x,y,z) * rotationMatrix;
        }

        public void Rotate(Matrix4x4 rotM) {
            rotationMatrix = rotM * rotationMatrix;
        }

        public void Translate(float x, float y, float z) {
            this.position += new Vector3(x, y, z);
        }

        public void Scale(float x, float y, float z) {
            this.scale += new Vector3(x, y, z);
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
            return this.rotationMatrix;
        }
    }
}
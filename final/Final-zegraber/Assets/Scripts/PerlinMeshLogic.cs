using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05
{
    public class PerlinMeshLogic : MeshLogic
    {
        [SerializeField] public Texture2D texture;

        public override void SetMesh(Mesh newMesh) {
            this.mesh = newMesh;
            this.mesh.RecalculateNormals();
            this.mesh.RecalculateTangents();

            this.meshRenderer = GetComponent<MeshRenderer>();
            this.meshFilter = GetComponent<MeshFilter>();
            this.meshFilter.mesh = this.mesh;

            this.material = new Material(this.shader);
            this.meshRenderer.material = this.material;
        }
    }
}
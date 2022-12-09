using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05
{
    public class CubeMesh : MonoBehaviour
    {
        [SerializeField] public MeshLogic meshLogic;

        void Start()
        {
            meshLogic.SetMesh(MeshLogic.GetCubeMesh());
        }
    }
}
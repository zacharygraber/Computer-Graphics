using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField, Range(0.5f, 10.0f)] public float rotationRadius = 4;

    private Transform _transform;
    private Vector2 prevMousePos;
    new public Transform transform 
    {
        get
        {
            return _transform ?? (_transform = GetComponent<Transform>());
        }
    }

    private void Awake()
    {
        prevMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // If the mouse is held down, we want the object to rotate
            Vector2 mousePos = Input.mousePosition;

            float dx = (mousePos - prevMousePos).x;
            float dy = (mousePos - prevMousePos).y;

            float dr = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
            Vector3 n = Vector3.Normalize(new Vector3(-1*(dy / dr), dx/dr, 0));
            float Theta = -1 * dr / rotationRadius;


            transform.RotateAround(transform.position, n, Theta);

            
        }

        prevMousePos = Input.mousePosition;
    }
}

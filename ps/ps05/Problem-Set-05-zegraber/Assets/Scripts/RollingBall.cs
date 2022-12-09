using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class RollingBall : MonoBehaviour
    {
        public enum InteractionMode {
            Rotate,
            TranslateXY,
            TranslateXZ,
            Scale
        }

        [SerializeField, Range(0.5f, 500.0f)] public float sensitivity = 30.0f;
        [SerializeField] public static InteractionMode currentMode = InteractionMode.Rotate;
        [SerializeField] private CustomTransform customTransform;

        private Vector2 prevMousePos;
        private bool prevFocused; // tracks whether the window was in focus last frame, and if not, this value is used to ignore the next input.

        private void Awake()
        {
            prevMousePos = Input.mousePosition;
            prevFocused = false;
        }

        void Update()
        {
            if (Application.isFocused && !prevFocused) {
                prevFocused = Application.isFocused;
                prevMousePos = Input.mousePosition;
                return;
            }

            if (Input.GetMouseButton(0))
            {
                // If the mouse is held down, we want the object to rotate
                Vector2 mousePos = Input.mousePosition;
                float dx = (mousePos - prevMousePos).x;
                float dy = (mousePos - prevMousePos).y;

                switch (currentMode) {
                    case InteractionMode.Rotate:
                        float dr = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
                        Vector3 n = Vector3.Normalize(new Vector3(-1*(dy / dr), dx/dr, 0));
                        float Theta = dr / sensitivity;

                        // Rotate around the unit vector n by Theta degrees
                        float c = Mathf.Cos(Theta);
                        float s = Mathf.Sin(Theta);
                        float t = 1.0f - c;

                        Matrix4x4 newRotation = new Matrix4x4(
                            new Vector4(t*n.x*n.x + c, t*n.x*n.y + n.z*s, t*n.x*n.z - n.y*s, 0),
                            new Vector4(t*n.x*n.y - n.z*s, t*n.y*n.y + c, t*n.y*n.z + n.x*s, 0),
                            new Vector4(t*n.x*n.z + n.y*s, t*n.y*n.z - n.x*s, t*n.z*n.z + c, 0),
                            new Vector4(0,0,0,1)
                        );
                        customTransform.Rotate(newRotation);
                        break;
                    
                    case InteractionMode.TranslateXY:
                        customTransform.Translate(dx * (1.0f / sensitivity), dy * (1.0f / sensitivity), 0);
                        break;

                    case InteractionMode.TranslateXZ:
                        customTransform.Translate(dx * (1.0f / sensitivity), 0, -1* dy * (1.0f / sensitivity));
                        break;

                    case InteractionMode.Scale:
                        // Scale uniformly by the y component of mouse movement
                        // up bigger, down smaller.
                        float scaleFactor = dy * (1.0f / sensitivity);
                        customTransform.Scale(scaleFactor, scaleFactor, scaleFactor);
                        break;
                }
            }

            prevMousePos = Input.mousePosition;
            prevFocused = Application.isFocused;
           
        }

        public static void SetModeRotate() {
            currentMode = InteractionMode.Rotate;
        }

        public static void SetModeTranslateXY() {
            currentMode = InteractionMode.TranslateXY;
        }

        public static void SetModeTranslateXZ() {
            currentMode = InteractionMode.TranslateXZ;
        }

        public static void SetModeScale() {
            currentMode = InteractionMode.Scale;
        }
    }
}
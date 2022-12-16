using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {

    public class LightToggle : MonoBehaviour
    {
        [SerializeField] private CustomLight _light;
        [SerializeField] private CustomTransform _customTransform;
        [SerializeField, Range(0.0f, 0.1f)] private float colliderRadius;
        private float lightIntensity;
        private Color diffuseColor; // Diffuse color controls light representation in-world color
        private bool on = true;

        // Start is called before the first frame update
        void Start()
        {
            lightIntensity = _light.lightPower;
            diffuseColor = _light.diffuseColor;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                Vector2 lightScreenPos = worldSpaceToScreen(_customTransform.GetPositionAsVector4());
                Vector2 normalizedMousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
                normalizedMousePos = (2 * normalizedMousePos) - new Vector2(1,1);
                float sqrDistance = (normalizedMousePos - lightScreenPos).sqrMagnitude;
                if (sqrDistance <= colliderRadius) {
                    if (on == true) {
                        _light.lightPower = 0;
                        _light.diffuseColor = Color.black;
                        on = false;
                    }
                    else {
                        _light.lightPower = lightIntensity;
                        _light.diffuseColor = diffuseColor;
                        on = true;
                    }
                }
            }
        }

        // Convert a world-space coordinate to screen space
        private Vector2 worldSpaceToScreen(Vector4 pos) {
            CustomCamera cam = FindObjectOfType<CustomCamera>();
            Matrix4x4 P = cam.projectionMatrix;
            Matrix4x4 V = cam.viewMatrix;
            Matrix4x4 M = new Matrix4x4(
                new Vector4(1,0,0,0),
                new Vector4(0,1,0,0),
                new Vector4(0,0,1,0),
                new Vector4(_customTransform.position.x, _customTransform.position.y, _customTransform.position.z,1)
            ) * new Matrix4x4(
                new Vector4(_customTransform.scale.x == 0 ? 0.001f : _customTransform.scale.x,0,0,0), // ensure we never scale by 0
                new Vector4(0,_customTransform.scale.y == 0 ? 0.001f : _customTransform.scale.y,0,0),
                new Vector4(0,0,_customTransform.scale.z == 0 ? 0.001f : _customTransform.scale.z,0), 
                new Vector4(0,0,0,1)
            );

            Vector4 newPos4 = P*V*M * pos; 

            return new Vector2(newPos4.x / newPos4.w, newPos4.y / newPos4.w);
        }
    }
}
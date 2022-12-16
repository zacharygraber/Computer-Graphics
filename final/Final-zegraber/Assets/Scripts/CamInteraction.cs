using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS05 {
    public class CamInteraction : MonoBehaviour
    {
        private enum InteractionMode {
            FirstPerson,
            Orbit,
            RotateWorld
        }

        [SerializeField] private CustomTransform waterTransform, terrainTransform;
        [SerializeField, Range(0.5f, 500.0f)] private float sensitivity = 30.0f;
        [SerializeField, Range(0.01f, 5.0f)] private float firstPersonMoveSpeed = 0.5f;
        [SerializeField] private InteractionMode currentMode = InteractionMode.Orbit;
        [SerializeField] private GameObject helpScreen;

        private CustomCamera cam;
        private CustomLight[] lights;
        [SerializeField] private bool enableInput = true;

        const float ORBIT_RAD = 20.0f;
        const float ORBIT_SPEED = 0.25f;
        const float ORBIT_HEIGHT = 16.0f;
        const float ORBIT_XANG = 35.0f;

        const float ROTATE_WORLD_XANG = 30.0f;
        const float ROTATE_WORLD_ZOOM_SENS = 1.0f;

        private Vector3 FIRST_PERSON_STARTING_POS = new Vector3(0,4,3);

        // Start is called before the first frame update
        void Start()
        {
            cam = FindObjectOfType<CustomCamera>();
            lights = (FindObjectOfType<MeshLogic>()).lights;

            switch (currentMode) {
                case InteractionMode.Orbit:
                    SwitchToOrbit();
                    break;
                case InteractionMode.RotateWorld:
                    SwitchToRotateWorld();
                    break;
                case InteractionMode.FirstPerson:
                    SwitchToFirstPerson();
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("c")) {
                toggleCameraMode();
            }

            if (Input.GetKeyDown("h")) {
                toggleHelp();
            }

            Vector2 mousePos = Input.mousePosition;
            if (enableInput) {
                float dx = Input.GetAxis("Mouse X");
                float dy = Input.GetAxis("Mouse Y");
                switch (currentMode) {
                    case InteractionMode.Orbit:
                        float angle = -1* Time.time * ORBIT_SPEED;
                        cam._customTransform.position.x = ORBIT_RAD * Mathf.Cos(angle + Mathf.PI/2);
                        cam._customTransform.position.z = ORBIT_RAD * Mathf.Sin(angle + Mathf.PI/2);
                        cam._customTransform.rotation = new Vector3(ORBIT_XANG, -1*angle*Mathf.Rad2Deg, 0);
                        break;

                    case InteractionMode.RotateWorld:
                        if (Input.GetMouseButton(1)) {
                            // If the RMB is held down, we want the object to rotate
                            angle = dx * (sensitivity / 100);
                            waterTransform.Rotate(0, angle, 0);
                            terrainTransform.Rotate(0, angle, 0);

                            // Rotate all the static lights along with the scene
                            for (int i = 0; i < lights.Length; i++) {
                                if (!(lights[i].animate)) {
                                    // Rotate them around the origin in the x-z plane
                                    Vector3 lightPos3 = lights[i].getPosition();
                                    Vector4 lightPos4 = new Vector4(lightPos3.x, lightPos3.y, lightPos3.z, 1);
                                    lights[i].setPosition((Vector3) (CustomTransform.ComputeRotationMatrix(0,angle,0) * lightPos4));
                                }
                            }
                        }
                        // Handle zooming in/out
                        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) {
                            cam.moveInCameraForwardDirection(ROTATE_WORLD_ZOOM_SENS);
                        }
                        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f ) {
                            cam.moveInCameraForwardDirection(-1*ROTATE_WORLD_ZOOM_SENS);
                        }
                        break;

                    case InteractionMode.FirstPerson:
                        float leftRightAngle = -1*dx * (sensitivity / 100);
                        float upDownAngle = -1*dy * (sensitivity / 100);
                        cam._customTransform.RotateClampX(upDownAngle, leftRightAngle, 0);

                        if (Input.GetKey("w") || Input.GetKey("up")) {
                            cam.moveInCameraForwardDirectionXZ(firstPersonMoveSpeed/100);
                        }
                        if (Input.GetKey("s") || Input.GetKey("down")) {
                            cam.moveInCameraForwardDirectionXZ(-1*firstPersonMoveSpeed/100);
                        }
                        if (Input.GetKey("a") || Input.GetKey("left")) {
                            cam.moveInCameraLeftDirectionXZ(firstPersonMoveSpeed/100);
                        }
                        if (Input.GetKey("d") || Input.GetKey("right")) {
                            cam.moveInCameraRightDirectionXZ(firstPersonMoveSpeed/100);
                        }
                        if (Input.GetKey("space")) {
                            cam._customTransform.Translate(0,firstPersonMoveSpeed/100,0);
                        }
                        if (Input.GetKey(KeyCode.LeftShift)) {
                            cam._customTransform.Translate(0,-1*firstPersonMoveSpeed/100,0);
                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) {
                            cam.fovY++;
                        }
                        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f ) {
                            cam.fovY--;
                        }
                        break;
                }
            }
        }

        void SwitchToOrbit() {
            cam._customTransform.position = new Vector3(0, ORBIT_HEIGHT, ORBIT_RAD);
            cam._customTransform.rotation = new Vector3(ORBIT_XANG, 0, 0);
            Cursor.lockState = CursorLockMode.None;
            cam.fovY = 70;
            currentMode = InteractionMode.Orbit;
        }

        void SwitchToRotateWorld() {
            cam._customTransform.position = new Vector3(0, ORBIT_HEIGHT, ORBIT_RAD);
            cam._customTransform.rotation = new Vector3(ROTATE_WORLD_XANG, 0, 0);
            Cursor.lockState = CursorLockMode.None;
            cam.fovY = 70;
            currentMode = InteractionMode.RotateWorld;
        }

        void SwitchToFirstPerson() {
            cam._customTransform.position = FIRST_PERSON_STARTING_POS;
            cam._customTransform.rotation = Vector3.zero;
            Cursor.lockState = CursorLockMode.Locked;
            currentMode = InteractionMode.FirstPerson;
        }

        public void disableInputs() {
            enableInput = false;
            Cursor.lockState = CursorLockMode.None;
        }

        public void enableInputs() {
            enableInput = true;
            if (currentMode == InteractionMode.FirstPerson) {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void toggleCameraMode() {
            switch (currentMode) {
                case InteractionMode.Orbit:
                    SwitchToRotateWorld();
                    break;
                case InteractionMode.RotateWorld:
                    SwitchToFirstPerson();
                    break;
                case InteractionMode.FirstPerson:
                    SwitchToOrbit();
                    break;
            }
        }

        public void toggleHelp() {
            if (helpScreen.activeSelf) {
                helpScreen.SetActive(false);
                enableInputs();
            }
            else {
                helpScreen.SetActive(true);
                disableInputs();
            }
        }
    }
}
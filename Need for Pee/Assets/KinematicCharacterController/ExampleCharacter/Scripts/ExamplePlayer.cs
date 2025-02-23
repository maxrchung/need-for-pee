﻿using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public TextManagerScript TextManager;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        private int _textArrayIndex = -1;
        private bool _disabled = false;
        private IInteractable _interactable = null;
        private GameObject glowLight;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());

            glowLight = GameObject.Find("GlowLight");
        }

        public void Disable()
        {
            Cursor.lockState = CursorLockMode.None;
            _disabled = true;
             PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();
            characterInputs.MoveAxisForward = 0f;
            characterInputs.MoveAxisRight = 0f;
            Character.SetInputs(ref characterInputs);
        }

        public void Enable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _disabled = false;
        }

        public bool GetDisabled()
        {
            return _disabled;
        }

        private void Update()
        {
            var hasHit = false;
            if (Physics.Raycast(CharacterCamera.Transform.position, CharacterCamera.Transform.forward,
                    out RaycastHit hit, 4f))
            {
                _interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                if (_interactable != null && _interactable.CanInteract())
                {
                    if (_textArrayIndex == -1)
                    {
                        _textArrayIndex =
                            TextManager.PissTextGeneration("Press P to interact", new Vector2(Screen.width * 0.15f, Screen.height * 0.4f), 0.2f, false);
                        glowLight.transform.position = new Vector3(
                            hit.collider.gameObject.transform.position.x,
                            glowLight.transform.position.y,
                            hit.collider.gameObject.transform.position.z
                        );
                    }

                    hasHit = true;
                }
            }

            if (!hasHit && _textArrayIndex != -1)
            {
                TextManager.ClearText(_textArrayIndex);
                _textArrayIndex = -1;
                _interactable = null;
                glowLight.transform.position = new Vector3(0, glowLight.transform.position.y, -30);
            }

            if (_disabled)
                return;
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection =
                    Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation *
                    CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3
                    .ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
            scrollInput = 0f;

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetKeyDown(KeyCode.F5))
            {
                CharacterCamera.TargetDistance =
                    (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            //characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            //characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            //characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);

            if (_interactable != null && Input.GetKeyDown(KeyCode.P))
            {
                _interactable.Interact();
            }
        }
    }
}
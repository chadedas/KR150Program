using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    public class XRInputController : GGMonoBehaviour
    {
        #region Variables

        private enum UpdateType
        {
            Update,
            BeforeRender,
            UpdateAndBeforeRender
        }

        [SerializeField, RequiredField]
        private PlayerInput playerInput;
        
        [SerializeField]
        private UpdateType trackingType = UpdateType.UpdateAndBeforeRender;
        
        [SerializeField, RequiredField]
        private Transform leftController;
        
        [SerializeField, RequiredField]
        private Transform rightController;
        
        [SerializeField, ReadOnly, LineSeparator]
        private XRInputs leftControllerInputs;
        
        [SerializeField, ReadOnly]
        private XRTracking leftControllerTracking;
        
        [SerializeField, ReadOnly]
        private XRInputs rightControllerInputs;
        
        [SerializeField, ReadOnly]
        private XRTracking rightControllerTracking;

        #endregion

        //---------------------------------------------------------------------------------------------------

        #region Game Loop

        private void OnEnable()
        {
            Application.onBeforeRender += OnBeforeRender;
        }

        private void OnDisable()
        {
            Application.onBeforeRender -= OnBeforeRender;
        }

        private void Update()
        {
            if (trackingType == UpdateType.Update || trackingType == UpdateType.UpdateAndBeforeRender)
            {
                HandleTracking();
            }
            UpdateUserInputs();
        }

        private void OnBeforeRender()
        {
            if (trackingType == UpdateType.BeforeRender || trackingType == UpdateType.UpdateAndBeforeRender)
            {
                HandleTracking();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region User Inputs

        private void UpdateUserInputs()
        {
            // left hand
            if (leftControllerInputs != null)
            {
                leftControllerInputs.menuButton.keydown = playerInput.actions["Left_MenuButton"].WasPressedThisFrame();
                leftControllerInputs.menuButton.keypressed = playerInput.actions["Left_MenuButton"].IsPressed();
                leftControllerInputs.menuButton.keyreleased = playerInput.actions["Left_MenuButton"].WasReleasedThisFrame();

                leftControllerInputs.primaryButton.keytouched = playerInput.actions["Left_PrimaryTouch"].IsPressed();
                leftControllerInputs.primaryButton.keydown = playerInput.actions["Left_PrimaryButton"].WasPressedThisFrame();
                leftControllerInputs.primaryButton.keypressed = playerInput.actions["Left_PrimaryButton"].IsPressed();
                leftControllerInputs.primaryButton.keyreleased = playerInput.actions["Left_PrimaryButton"].WasReleasedThisFrame();

                leftControllerInputs.secondaryButton.keytouched = playerInput.actions["Left_SecondaryTouch"].IsPressed();
                leftControllerInputs.secondaryButton.keydown = playerInput.actions["Left_SecondaryButton"].WasPressedThisFrame();
                leftControllerInputs.secondaryButton.keypressed = playerInput.actions["Left_SecondaryButton"].IsPressed();
                leftControllerInputs.secondaryButton.keyreleased = playerInput.actions["Left_SecondaryButton"].WasReleasedThisFrame();

                leftControllerInputs.joystickButton.keytouched = playerInput.actions["Left_JoystickTouch"].IsPressed();
                leftControllerInputs.joystickButton.keydown = playerInput.actions["Left_JoystickButton"].WasPressedThisFrame();
                leftControllerInputs.joystickButton.keypressed = playerInput.actions["Left_JoystickButton"].IsPressed();
                leftControllerInputs.joystickButton.keyreleased = playerInput.actions["Left_JoystickButton"].WasReleasedThisFrame();

                leftControllerInputs.joystickHorizontal = playerInput.actions["Left_JoystickAxis"].ReadValue<Vector2>().x;
                leftControllerInputs.joystickVertical = playerInput.actions["Left_JoystickAxis"].ReadValue<Vector2>().y;

                leftControllerInputs.triggerTouch = playerInput.actions["Left_TriggerTouch"].IsPressed();
                leftControllerInputs.triggerAxis = playerInput.actions["Left_TriggerAxis"].ReadValue<float>();

                leftControllerInputs.gripAxis = playerInput.actions["Left_GripAxis"].ReadValue<float>();
            }

            // right hand
            if (rightControllerInputs != null)
            {
                rightControllerInputs.menuButton.keydown = playerInput.actions["Right_MenuButton"].WasPressedThisFrame();
                rightControllerInputs.menuButton.keypressed = playerInput.actions["Right_MenuButton"].IsPressed();
                rightControllerInputs.menuButton.keyreleased = playerInput.actions["Right_MenuButton"].WasReleasedThisFrame();

                rightControllerInputs.primaryButton.keytouched = playerInput.actions["Right_PrimaryTouch"].IsPressed();
                rightControllerInputs.primaryButton.keydown = playerInput.actions["Right_PrimaryButton"].WasPressedThisFrame();
                rightControllerInputs.primaryButton.keypressed = playerInput.actions["Right_PrimaryButton"].IsPressed();
                rightControllerInputs.primaryButton.keyreleased = playerInput.actions["Right_PrimaryButton"].WasReleasedThisFrame();

                rightControllerInputs.secondaryButton.keytouched = playerInput.actions["Right_SecondaryTouch"].IsPressed();
                rightControllerInputs.secondaryButton.keydown = playerInput.actions["Right_SecondaryButton"].WasPressedThisFrame();
                rightControllerInputs.secondaryButton.keypressed = playerInput.actions["Right_SecondaryButton"].IsPressed();
                rightControllerInputs.secondaryButton.keyreleased = playerInput.actions["Right_SecondaryButton"].WasReleasedThisFrame();

                rightControllerInputs.joystickButton.keytouched = playerInput.actions["Right_JoystickTouch"].IsPressed();
                rightControllerInputs.joystickButton.keydown = playerInput.actions["Right_JoystickButton"].WasPressedThisFrame();
                rightControllerInputs.joystickButton.keypressed = playerInput.actions["Right_JoystickButton"].IsPressed();
                rightControllerInputs.joystickButton.keyreleased = playerInput.actions["Right_JoystickButton"].WasReleasedThisFrame();

                rightControllerInputs.joystickHorizontal = playerInput.actions["Right_JoystickAxis"].ReadValue<Vector2>().x;
                rightControllerInputs.joystickVertical = playerInput.actions["Right_JoystickAxis"].ReadValue<Vector2>().y;

                rightControllerInputs.triggerTouch = playerInput.actions["Right_TriggerTouch"].IsPressed();
                rightControllerInputs.triggerAxis = playerInput.actions["Right_TriggerAxis"].ReadValue<float>();

                rightControllerInputs.gripAxis = playerInput.actions["Right_GripAxis"].ReadValue<float>();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Tracking

        private void HandleTracking()
        {
            if(!playerInput) { return; }
            
            HandleTracking_LeftController();
            HandleTracking_RightController();
        }

        private void HandleTracking_LeftController()
        {
            if(leftController)
            {
                // cache values
                leftControllerTracking.isTracked = playerInput.actions["Left_IsTracked"].ReadValue<float>();
                leftControllerTracking.position = playerInput.actions["Left_Position"].ReadValue<Vector3>();
                leftControllerTracking.rotation = playerInput.actions["Left_Rotation"].ReadValue<Quaternion>();

                // apply values
                if (1 <= leftControllerTracking.isTracked)
                {
                    leftController.localPosition = leftControllerTracking.position;
                    leftController.localRotation = leftControllerTracking.rotation;
                }
            }
        }

        private void HandleTracking_RightController()
        {
            if(rightController)
            {
                // cache values
                rightControllerTracking.isTracked = playerInput.actions["Right_IsTracked"].ReadValue<float>();
                rightControllerTracking.position = playerInput.actions["Right_Position"].ReadValue<Vector3>();
                rightControllerTracking.rotation = playerInput.actions["Right_Rotation"].ReadValue<Quaternion>();

                // apply values
                if (1 <= rightControllerTracking.isTracked)
                {
                    rightController.localPosition = rightControllerTracking.position;
                    rightController.localRotation = rightControllerTracking.rotation;
                }
            }
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
        #region Public Functions

        public XRInputs GetLeftHandInputs()
        {
            return leftControllerInputs;
        }

        public XRInputs GetRightHandInputs()
        {
            return rightControllerInputs;
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
        #region Getter / Setter
        
        public PlayerInput PlayerInput
        {
            get { return playerInput; }
            set{ playerInput = value; }
        }

        #endregion

    } // class end
}
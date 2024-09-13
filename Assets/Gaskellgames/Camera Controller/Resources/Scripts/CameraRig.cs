using System.Collections;
using UnityEngine;
using Gaskellgames.InputEventSystem;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraRig : GGMonoBehaviour
    {
        #region Variables
        
        [SerializeField]
        [Tooltip("Registered cameraRigs are added to the global camera list when enabled and removed when disabled")]
        private bool registerCamera = true;
        
        [SerializeField]
        private bool freeFlyCamera;
        
        [SerializeField]
        private bool cameraShake;
        
        [SerializeField, LineSeparator, RequiredField]
        private GMKInputController gmkInputController;
        
        [SerializeField, Range(0, 100)]
        private float moveSpeed = 10;
        
        [SerializeField, Range(0, 100)]
        private float boostSpeed = 25;
        
        [SerializeField, Range(0, 100)]
        private int xSensitivity = 50;
        
        [SerializeField, Range(0, 100)]
        private int ySensitivity = 50;
        
        [SerializeField]
        private bool freeFlyActive;
        
        [SerializeField, LineSeparator]
        [Tooltip("If the freelookRig is assigned, then the cameraRig will ignore follow, lookAt, turnSpeed & followOffset")]
        private CameraFreelookRig freelookRig;
        
        [SerializeField, Space]
        private Transform follow;
        
        [SerializeField]
        private Transform lookAt;
        
        [SerializeField, Range(0, 1)]
        private float turnSpeed = 1.0f;
        
        [SerializeField]
        private Vector3 followOffset;
        
        [SerializeField, LineSeparator, Range(0, 1)]
        [Tooltip("Smoothing value effects the fade time of the shake effect")]
        private float shakeSmoothing = 1;
        
        [SerializeField, Range(0, 10)]
        [Tooltip("How much the camera can move when receiving the full intensity of an incoming camera shaker effect")]
        private float positionMagnitude = 7.5f;
        
        [SerializeField, Range(0, 10)]
        [Tooltip("How much the camera can rotate when receiving the full intensity of an incoming camera shaker effect")]
        private float rotationMagnitude = 5;
        
        [SerializeField, LineSeparator]
        private CameraLens lens = new CameraLens();
        
        private UserInputs userInputs;
        private float horizontalInput;
        private float verticalInput;
        private float heightInput;
        private float xRotation;
        private float yRotation;
        private bool isBoosting;
        private bool isRising;
        private bool isFalling;
        
        private Quaternion rotationTarget;
        private Vector3 direction;
        
        private SubTransform shakeOffset;
        private SubTransform currentShake;
        private SubTransform previousShake;
        private float shakeMultiplier;
        
        private float lerpSpeedMultiplier = 40f;
        private float desiredCameraTilt;
        
        #endregion

        //----------------------------------------------------------------------------------------------------

        #region On Events

        private void OnEnable()
        {
            if(registerCamera)
            {
                CameraList.Register(this);
            }
            
            if(cameraShake)
            {
                CameraList.SetShakable(this);
            }
        }

        private void OnDisable()
        {
            CameraList.Unregister(this);
            CameraList.UnsetShakable(this);
        }

        private void OnDestroy()
        {
            CameraList.Unregister(this);
            CameraList.UnsetShakable(this);
        }
        
        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop
        
        private void Start()
        {
            shakeOffset = new SubTransform();
            currentShake = new SubTransform();
            previousShake = new SubTransform();
        }

        private void Update()
        {
            if (freeFlyCamera)
            {
                UpdateFreeFlyCamera();
            }
            else
            {
                UpdateCamera();
                HandleCameraShake();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        
        #region Gizmos / Debug [Editor]

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if(!freeFlyCamera)
            {
                UpdateCamera();
            }
        }

        protected override void OnDrawGizmosConditional(bool selected)
        {
            // draw camera frustum
            if (lens.showFrustrum)
            {
                Matrix4x4 resetMatrix = Gizmos.matrix;
                Gizmos.matrix = gameObject.transform.localToWorldMatrix;
                Gizmos.color = Color.grey;

                float width = Screen.width * 1.000f;
                float height = Screen.height * 1.000f;
                float aspect = width / height;

                Gizmos.DrawFrustum(Vector3.zero, lens.verticalFOV, lens.farClipPlane, lens.nearClipPlane, aspect);
                Gizmos.matrix = resetMatrix;
            }
        }

        #endregion
        
#endif

        //----------------------------------------------------------------------------------------------------

        #region User Inputs

        private void GetUserInputs()
        {
            if(gmkInputController)
            {
                userInputs = gmkInputController.GetUserInputs();
            }
        }

        private void HandleUserInput()
        {
            if (gmkInputController)
            {
                // movement
                horizontalInput = userInputs.inputAxisX;
                verticalInput = userInputs.inputAxisY;

                // look rotation
                float mouseX = userInputs.inputAxis4 * Time.unscaledDeltaTime * xSensitivity * 2;
                float mouseY = userInputs.inputAxis5 * Time.unscaledDeltaTime * ySensitivity * 2;
                yRotation += mouseX;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90, 90);
                
                // speed up
                isBoosting = userInputs.inputButton4.keypressed;
                
                // height
                isRising = userInputs.inputButton9.keypressed;
                isFalling = userInputs.inputButton8.keypressed;
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void UpdateFreeFlyCamera()
        {
            if (freeFlyActive)
            {
                GetUserInputs();
                HandleUserInput();
                HandleFreeFlyMovement();
            }
        }
        
        private void HandleFreeFlyMovement()
        {
            // calculate movement input direction
            if (isRising && !isFalling)
            {
                heightInput = 1;
            }
            else if (isFalling && !isRising)
            {
                heightInput = -1;
            }
            else
            {
                heightInput = 0;
            }
            Vector3 inputDirection = (Vector3.forward * verticalInput) + (Vector3.right * horizontalInput) + (Vector3.up * heightInput);
            
            // calculate rotation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            
            // calculate acceleration
            float speed;
            if (isBoosting)
            {
                speed = boostSpeed;
            }
            else
            {
                speed = moveSpeed;
            }
            float acceleration = speed * Time.unscaledDeltaTime;
            
            // move object
            transform.Translate(inputDirection * acceleration);
        }
        
        private void UpdateCamera()
        {
            // update position
            if (freelookRig)
            {
                transform.position = freelookRig.GetFreelookCameraPosition();
            }
            else if (follow)
            {
                transform.position = follow.position + followOffset;
            }

            // update rotation
            bool updateRotation = false;
            if (lookAt)
            {
                direction = (lookAt.position - transform.position).normalized;
                updateRotation = true;
            }
            else if (freelookRig)
            {
                direction = (freelookRig.transform.position - transform.position).normalized;
                updateRotation = true;
            }
            
            if (updateRotation)
            {
                rotationTarget = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, turnSpeed);
            }
            
            // update tilt
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, lens.tilt);
        }

        private IEnumerator SmoothlyLerpCameraTilt()
        {
            // smoothly lerp moveSpeed to moveState speed
            float time = 0;
            float difference = Mathf.Abs(desiredCameraTilt - lens.tilt);
            float startValue = lens.tilt;

            while (time < difference)
            {
                lens.tilt = Mathf.Lerp(startValue, desiredCameraTilt, time / difference);
                time += Time.deltaTime * lerpSpeedMultiplier;
                yield return null;
            }

            lens.tilt = desiredCameraTilt;
        }

        private void HandleCameraShake()
        {
            if (cameraShake)
            {
                float adjustedSmoothing = 0.5f + (shakeSmoothing * 0.5f);
                float shakePower = Mathf.Pow(shakeMultiplier, adjustedSmoothing);
                
                if(shakePower > 0)
                {
                    // update previous
                    previousShake.position = currentShake.position;
                    previousShake.eulerAngles = currentShake.eulerAngles;
                    
                    // generate position values
                    float positionX = positionMagnitude * Random.Range(-0.2f, 0.2f);
                    float positionY = positionMagnitude * Random.Range(-0.2f, 0.2f);
                    float positionZ = positionMagnitude * Random.Range(-0.2f, 0.2f);
                    currentShake.position = MathUtility.RoundVector3(new Vector3(positionX, positionY, positionZ) * shakePower, 3);
                    
                    // generate angle values
                    float angleX = rotationMagnitude * Random.Range(-2.0f, 2.0f);
                    float angleY = rotationMagnitude * Random.Range(-2.0f, 2.0f);
                    float angleZ = rotationMagnitude * Random.Range(-2.0f, 2.0f);
                    currentShake.eulerAngles = MathUtility.RoundVector3(new Vector3(angleX, angleY, angleZ) * shakePower, 3);
                    
                    // add shake for this frame (currentShake) and counteract shake from previous frame (previousShake)
                    transform.position += currentShake.position - previousShake.position;
                    transform.eulerAngles += currentShake.eulerAngles - previousShake.eulerAngles;
                    shakeMultiplier = Mathf.Clamp01(shakeMultiplier - Time.deltaTime);
                }
                else if (!(shakeOffset.position == Vector3.zero && shakeOffset.eulerAngles == Vector3.zero))
                {
                    // counteract shake from previous frame (previousShake)
                    transform.position -= currentShake.position;
                    transform.eulerAngles -= currentShake.eulerAngles;
                    currentShake.position = Vector3.zero;
                    currentShake.eulerAngles = Vector3.zero;
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Public Functions
        
        public void ShakeCamera(float intensity)
        {
            shakeMultiplier = Mathf.Clamp01(shakeMultiplier + intensity);
        }
        
        public void TiltCamera(float newCameraTilt)
        {
            StopAllCoroutines();
            desiredCameraTilt = newCameraTilt;
            StartCoroutine(SmoothlyLerpCameraTilt());
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Getters / Setters

        public bool IsFreeFlyActive
        {
            get { return freeFlyActive; }
            set { freeFlyActive = value;}
        }

        public bool IsFreeFlyCamera
        {
            get { return freeFlyCamera; }
            set { freeFlyCamera = value;}
        }

        public GMKInputController GMKInputController
        {
            get { return gmkInputController; }
            set { gmkInputController = value; }
        }

        public CameraFreelookRig FreelookRig
        {
            get { return freelookRig; }
            set { freelookRig = value; }
        }

        public Transform CameraFollow
        {
            get { return follow; }
            set { follow = value; }
        }

        public Transform LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

        public CameraLens Lens
        {
            get { return lens; }
            set { lens = value; }
        }
        
        #endregion

    } //class end
}

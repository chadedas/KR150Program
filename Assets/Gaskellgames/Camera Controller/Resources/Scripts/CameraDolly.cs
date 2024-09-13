using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraDolly : GGMonoBehaviour
    {
        #region Variables
        
        private enum UpdateState
        {
            Initialisation,
            UpdateTargetValues,
            UpdatePosition,
        }
        
        [SerializeField]
        private int startIndex;
        
        [SerializeField]
        private bool flipStartDirection;
        
        [SerializeField]
        private bool updateRotation;
        
        [SerializeField, LineSeparator, RequiredField]
        private CameraTrack cameraTrack;
        
        [SerializeField, RequiredField]
        private Transform lookAt;
        
        [SerializeField, Range(0, 1)]
        private float turnSpeed = 1.0f;
        
        [SerializeField, Range(0, 10)]
        private float moveSpeed = 2;
        
        private UpdateState updateState = UpdateState.Initialisation;
        private Rigidbody rb;
        
        private SubTransform startTransform;
        private SubTransform currentTransform;
        private SubTransform targetTransform;
        private bool isReturning;
        private float timeElapsed;
        private int currentIndex;
        private int targetIndex;
        
        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Reset()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = false;
        }

        private void Start()
        {
            updateState = UpdateState.Initialisation;
            InitialiseStartValues();
        }

        private void FixedUpdate()
        {
            if (cameraTrack != null)
            {
                // calculations
                if (updateState == UpdateState.UpdateTargetValues)
                {
                    UpdatePathInfo();
                }
            
                // movement
                if (updateState == UpdateState.UpdatePosition)
                {
                    if (UpdatePosition()) { updateState = UpdateState.UpdateTargetValues; }
                }

                if (updateRotation)
                {
                    UpdateRotation();
                }
            }
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Initialisation

        private void InitialiseStartValues()
        {
            // setup rigidbody
            rb = gameObject.GetComponent<Rigidbody>();
            
            // setup transforms
            startTransform = new SubTransform();
            currentTransform = new SubTransform();
            targetTransform = new SubTransform();
            
            // setup 'tracking values'
            if (cameraTrack != null)
            {
                rb.transform.position = cameraTrack.WaypointPosition(currentIndex);
                startTransform.position = rb.transform.position;
            }
            currentTransform.position = startTransform.position;

            // setup variables
            isReturning = flipStartDirection;
            currentIndex = startIndex;
            targetIndex = currentIndex;
            updateState = UpdateState.UpdateTargetValues;
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void UpdatePathInfo()
        {
            ResetTimers();
            UpdateIndex();
            UpdateTransforms();
            
            updateState = UpdateState.UpdatePosition;
        }
        
        private void ResetTimers()
        {
            timeElapsed = 0;
        }
        
        private void UpdateIndex()
        {
            currentIndex = targetIndex;
            targetIndex = GetNextIndex(currentIndex);
        }

        private int GetNextIndex(int index)
        {
            if(!cameraTrack.ClosedLoop)
            {
                if (index == cameraTrack.WaypointPositionCount() - 1)
                {
                    isReturning = true;
                }
                else if (index == 0)
                {
                    isReturning = false;
                }
            }
            
            if (isReturning)
            {
                if (cameraTrack.ClosedLoop && index == 0)
                {
                    index = cameraTrack.WaypointPositionCount() - 1;
                }
                else
                {
                    index--;
                }
            }
            else
            {
                if (cameraTrack.ClosedLoop && index == cameraTrack.WaypointPositionCount() - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }

            return index;
        }

        private void UpdateTransforms()
        {
            startTransform.position = currentTransform.position;
            targetTransform.position = cameraTrack.WaypointPosition(targetIndex);
        }
        
        private bool UpdatePosition()
        {
            // setup lerp speed values
            float lerpSpeed = (timeElapsed / Vector3.Distance(startTransform.position, targetTransform.position)) * moveSpeed;
            currentTransform.position = Vector3.Lerp(startTransform.position, targetTransform.position, lerpSpeed);

            // update platform and timer
            rb.transform.position = currentTransform.position;
            timeElapsed += Time.fixedDeltaTime;

            if (currentTransform.position != targetTransform.position)
            {
                return false;
            }
            else
            {
                // account for rounding errors
                currentTransform.position = targetTransform.position;
                return true;
            }
        }

        private void UpdateRotation()
        {
            if (lookAt != null)
            {
                Vector3 direction = (lookAt.position - transform.position).normalized;
                Quaternion rotationTarget = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, turnSpeed);
            }
            
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Getter / Setter

        public CameraTrack GetCameraTrack() { return cameraTrack; }
        
        public Transform GetLookAt() { return lookAt; }

        #endregion
        
    } // class end
}
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class ShadowTransform : MonoBehaviour
    {
        #region variables

        private enum UpdateType
        {
            Update,
            Fixedupdate
        }
        
        [SerializeField]
        [Tooltip("toggle whether follow, look at and rotate with should be updated in editor")]
        private bool updateInEditor;
        
        [SerializeField]
        [Tooltip("GameObject position will be set equal to the reference Transform position. Can be constrained to only follow on a specific axis")]
        private Transform follow;
        
        [SerializeField]
        [Tooltip("GameObject rotation will be set so that the forward rotation points towards the reference Transform")]
        private Transform lookAt;
        
        [SerializeField]
        [Tooltip("GameObject rotation will be set equal to the reference Transform rotation. Can be constrained to only rotate on a specific axis")]
        private Transform rotateWith;
        
        [SerializeField]
        private UpdateType updateType = UpdateType.Update;
        
        [SerializeField]
        private bool followX = true;
        
        [SerializeField]
        private bool followY = true;
        
        [SerializeField]
        private bool followZ = true;
        
        [SerializeField]
        private bool rotateWithX = true;
        
        [SerializeField]
        private bool rotateWithY = true;
        
        [SerializeField]
        private bool rotateWithZ = true;
        
        [SerializeField, Range(0, 1)]
        private float turnSpeed = 0.05f;
        
        private Quaternion rotationTarget;
        private Vector3 direction;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Update()
        {
            if (updateType == UpdateType.Update)
            {
                UpdatePosition();
                UpdateRotation();
            }
        }
        
        private void FixedUpdate()
        {
            if (updateType == UpdateType.Fixedupdate)
            {
                UpdatePosition();
                UpdateRotation();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        #region Editor Gizmos

        private void OnDrawGizmos()
        {
            if (updateInEditor & !Application.isPlaying)
            {
                UpdatePosition();
                UpdateRotation();
            }
        }

        #endregion

#endif
        
        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void UpdatePosition()
        {
            if (follow)
            {
                float newX = transform.position.x;
                float newY = transform.position.y;
                float newZ = transform.position.z;

                if (followX)
                {
                    newX = follow.position.x;
                }
                if (followY)
                {
                    newY = follow.position.y;
                }
                if (followZ)
                {
                    newZ = follow.position.z;
                }

                transform.position = new Vector3(newX, newY, newZ);
            }
        }
        
        private void UpdateRotation()
        {
            if (lookAt)
            {
                direction = (lookAt.position - transform.position).normalized;
                rotationTarget = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, turnSpeed);
            }
            else if (rotateWith)
            {
                float newX = transform.eulerAngles.x;
                float newY = transform.eulerAngles.y;
                float newZ = transform.eulerAngles.z;

                if (followX)
                {
                    newX = rotateWith.eulerAngles.x;
                }
                if (followY)
                {
                    newY = rotateWith.eulerAngles.y;
                }
                if (followZ)
                {
                    newZ = rotateWith.eulerAngles.z;
                }
                
                rotationTarget = Quaternion.Euler(newX, newY, newZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, turnSpeed);
            }
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Getter / Setter

        public Transform Follow
        {
            get { return follow; }
            set { follow = value; }
        }

        public bool FollowX
        {
            get { return followX; }
            set { followX = value; }
        }

        public bool FollowY
        {
            get { return followY; }
            set { followY = value; }
        }

        public bool FollowZ
        {
            get { return followZ; }
            set { followZ = value; }
        }

        public Transform LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

        public Transform RotateWith
        {
            get { return rotateWith; }
            set { rotateWith = value; }
        }

        public bool RotateWithX
        {
            get { return rotateWithX; }
            set { rotateWithX = value; }
        }

        public bool RotateWithY
        {
            get { return rotateWithY; }
            set { rotateWithY = value; }
        }

        public bool RotateWithZ
        {
            get { return rotateWithZ; }
            set { rotateWithZ = value; }
        }

        #endregion
        
    } // class end
}

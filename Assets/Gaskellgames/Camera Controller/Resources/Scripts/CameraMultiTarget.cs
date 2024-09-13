using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraMultiTarget : GGMonoBehaviour
    {
        #region Variables

        [SerializeField]
        private Color defaultColor = new Color32(000, 079, 223, 255);
        
        [SerializeField]
        private Color trackedColor = new Color32(000, 179, 223, 255);
        
        [SerializeField]
        private bool zoomCamera;
        
        [SerializeField, LineSeparator, RequiredField, Space]
        private Transform refCamLookAt;
        
        [SerializeField, Range(0, 10)]
        private float moveSpeed = 2.0f;
        
        [SerializeField, LineSeparator, RequiredField]
        private CameraRig cameraRig;
        
        [SerializeField]
        private bool boundsX;
        
        [SerializeField]
        private bool boundsY;
        
        [SerializeField]
        private bool boundsZ;
        
        [SerializeField, Range(0, 50)]
        private int padding = 10;
        
        [SerializeField, Range(0, 120)]
        private int minZoom = 50;
        
        [SerializeField, Range(0, 120)]
        private int maxZoom = 10;
        
        [SerializeField, Range(0, 10)]
        private float zoomSpeed = 2.0f;
        
        [SerializeField, LineSeparator]
        private List<Transform> targetObjects;
        
        private Vector3 defaultPosition;
        private Vector3 averagePosition;
        private Vector3 targetPosition;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Reset()
        {
            targetObjects = new List<Transform>();
        }

        private void Start()
        {
            if (refCamLookAt)
            {
                defaultPosition = refCamLookAt.position;
            }
        }

        private void Update()
        {
            UpdateRefCamLookAt();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        #region Editor Gizmos

        protected override void OnDrawGizmosConditional(bool selected)
        {
            if (refCamLookAt)
            {
                for (int i = 0; i < targetObjects.Count; i++)
                {
                    Gizmos.color = trackedColor;
                    Gizmos.DrawLine(refCamLookAt.position, targetObjects[i].position);
                }
            
                Gizmos.color = defaultColor;
                Gizmos.DrawLine(refCamLookAt.position, defaultPosition);
            }
        }

        #endregion

#endif
        
        //----------------------------------------------------------------------------------------------------

        #region Private Functions
        
        private void UpdateRefCamLookAt()
        {
            // calculate center point of the targets
            targetPosition = GetBoundsCenter();

            // move refCamLookAt to target position
            if (refCamLookAt.position != targetPosition + new Vector3(0.01f, 0.01f, 0.01f))
            {
                // lerp refCamLookAt position
                float step = moveSpeed * Time.deltaTime;
                refCamLookAt.position = Vector3.MoveTowards(refCamLookAt.position, targetPosition, step);
            }
            
            // update cameraRig zoom
            if (zoomCamera && cameraRig)
            {
                float newZoom = Mathf.Clamp(GetGreatestDistance() + padding, maxZoom, minZoom);
                float step = zoomSpeed * 10f * Time.deltaTime;
                cameraRig.Lens.verticalFOV = Mathf.Lerp(cameraRig.Lens.verticalFOV, newZoom, step);
            }
        }

        private Bounds GetBounds()
        {
            Bounds bounds = new Bounds(targetObjects[0].position, Vector3.zero);
            for (int i = 0; i < targetObjects.Count; i++)
            {
                bounds.Encapsulate(targetObjects[i].position);
            }

            return bounds;
        }

        private Vector3 GetBoundsCenter()
        {
            if (0 < targetObjects.Count)
            {
                if (1 == targetObjects.Count)
                {
                    return targetObjects[0].position;
                }
                else
                {
                    return GetBounds().center;
                }
            }
            else
            {
                return defaultPosition;
            }
        }

        private float GetGreatestDistance()
        {
            if (1 < targetObjects.Count)
            {
                float X = 0;
                float Y = 0;
                float Z = 0;
                
                if (boundsX) { X = GetBounds().size.x; }
                if (boundsX) { Y = GetBounds().size.y; }
                if (boundsX) { Z = GetBounds().size.z; }

                float maxAxis = Mathf.Max(X, Y);
                maxAxis = Mathf.Max(maxAxis, Z);
                
                return maxAxis;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Public Functions

        public void AddTargetToList(Transform newTarget)
        {
            if(!targetObjects.Contains(newTarget))
            {
                targetObjects.Add(newTarget);
            }
        }

        public void RemoveTargetFromList(Transform oldTarget)
        {
            if (targetObjects.Contains(oldTarget))
            {
                targetObjects.Remove(oldTarget);
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getter / Setter

        public void SetRefCamLookAt(Transform newRefCamLookAt) { refCamLookAt = newRefCamLookAt; }
        
        public bool BoundsX
        {
            get { return boundsX; }
            set { boundsX = value; }
        }
        
        public bool BoundsY
        {
            get { return boundsY; }
            set { boundsY = value; }
        }
        
        public bool BoundsZ
        {
            get { return boundsZ; }
            set { boundsZ = value; }
        }

        #endregion

    } //class end
}

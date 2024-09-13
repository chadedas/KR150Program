using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraShaker : GGMonoBehaviour
    {
        #region Variables
    
        [SerializeField]
        private Color32 gizmoColor = new Color32(000, 179, 223, 255);
        
        [SerializeField, Range(0, 1)]
        [Tooltip("Max intensity level of the shake effect")]
        private float intensity = 0.5f;
        
        [SerializeField, Range(1, 100)]
        [Tooltip("Max distance objects can be affected by this CameraShaker")]
        private float range = 10;
        
        [SerializeField, LineSeparator, ReadOnly]
        [Tooltip("List will be updated on component enable and at the point of activation")]
        private List<CameraRig> shakableRigs;
        
        #endregion
    
        //----------------------------------------------------------------------------------------------------

        #region On Events

        private void OnEnable()
        {
            UpdateTargetList();
        }

        private void OnDisable()
        {
            shakableRigs.Clear();
        }
        
        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
#if UNITY_EDITOR
        
        #region Gizmos [Editor]

        protected override void OnDrawGizmosConditional(bool selected)
        {
            Matrix4x4 resetMatrix = Gizmos.matrix;
            Gizmos.matrix = gameObject.transform.localToWorldMatrix;
    
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(Vector3.zero, new Vector3(range, 0, 0));
            Gizmos.DrawLine(Vector3.zero, new Vector3(-range, 0, 0));
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, range, 0));
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, -range, 0));
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, 0, range));
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, 0, -range));
            Gizmos.DrawWireSphere(Vector3.zero, range);
    
            Gizmos.matrix = resetMatrix;
        }
    
        #endregion
        
#endif
    
        //----------------------------------------------------------------------------------------------------

        #region Private Functions
        
        private void UpdateTargetList()
        {
            shakableRigs = new List<CameraRig>(CameraList.GetShakableRigList());
        }

        private void ShakeTargets()
        {
            for(int i = 0; i < shakableRigs.Count; ++i)
            {
                float distance = Vector3.Distance(transform.position, shakableRigs[i].transform.position);
                
                if (distance <= range)
                {
                    float adjustedDistance = Mathf.Clamp01(distance / range);
                    float adjustedIntensity = (1 - Mathf.Pow(adjustedDistance, 2)) * intensity;
                    shakableRigs[i].ShakeCamera(adjustedIntensity);
                }
            }
        }
        
        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Public Functions
        
        public void Activate()
        {
            UpdateTargetList();
            ShakeTargets();
        }

        #endregion
        
    } // class end
}
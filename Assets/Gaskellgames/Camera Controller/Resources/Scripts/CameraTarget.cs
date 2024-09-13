using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraTarget : GGMonoBehaviour
    {
        #region Variables

        enum TargetTypes
        {
            None,
            OnEnable,
            OnTrigger
        }

        [SerializeField]
        private TargetTypes targetType = TargetTypes.None;
        
        [SerializeField]
        private bool autoFindMultiTarget = true;
        
        [SerializeField]
        private CameraMultiTarget multiTarget;
        
        [Space, SerializeField]
        [Tooltip("Add a cameraBrain to set the active camera to the 'CameraSensor' cameraRig during OnTriggerEnter")]
        private CameraBrain cameraBrain;
        
        [SerializeField, TagDropdown]
        private string triggerTag = "";
        
        [SerializeField]
        private bool revertOnExit;
        
        [Space, SerializeField]
        private UnityEvent OnEnterTag;
        
        [Space, SerializeField]
        private UnityEvent OnExitTag;
        
        private CameraRig previousCamera;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Events

        private void OnEnable()
        {
            if(targetType == TargetTypes.OnEnable)
            {
                if (autoFindMultiTarget)
                {
                    multiTarget = FindObjectOfType<CameraMultiTarget>();
                }

                multiTarget.AddTargetToList(transform);
            }
        }

        private void OnDisable()
        {
            if (targetType == TargetTypes.OnEnable)
            {
                multiTarget.RemoveTargetFromList(transform);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (targetType == TargetTypes.OnTrigger)
            {
                if (other.CompareTag(triggerTag))
                {
                    if(cameraBrain != null)
                    {
                        CameraTriggerZone cameraTriggerZone = other.GetComponent<CameraTriggerZone>();
                        if (cameraTriggerZone != null)
                        {
                            previousCamera = cameraBrain.GetActiveCamera();
                            cameraBrain.SetActiveCamera(cameraTriggerZone.GetCameraRig());
                        }
                    }

                    CameraMultiTarget cameraMultiTarget = other.GetComponent<CameraMultiTarget>();
                    if(cameraMultiTarget != null)
                    {
                        cameraMultiTarget.AddTargetToList(transform);
                    }

                    OnEnterTag.Invoke();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (targetType == TargetTypes.OnTrigger)
            {
                if (other.CompareTag(triggerTag))
                {
                    if(revertOnExit)
                    {
                        cameraBrain.SetActiveCamera(previousCamera);
                    }

                    CameraMultiTarget cameraMultiTarget = other.GetComponent<CameraMultiTarget>();
                    if (cameraMultiTarget != null)
                    {
                        cameraMultiTarget.RemoveTargetFromList(transform);
                    }

                    OnExitTag.Invoke();
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getter / Setter

        public string GetTargetType()
        {
            return targetType.ToString();
        }

        #endregion

    } //class end
}

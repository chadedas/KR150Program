using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraTriggerZone : GGMonoBehaviour
    {
        #region Variables

        [SerializeField, RequiredField]
        private CameraRig cameraRig;
        
        [SerializeField]
        private Color32 triggerColour = new Color32(000, 179, 223, 079);
        
        [SerializeField]
        private Color32 triggerOutlineColour = new Color32(000, 179, 223, 128);
        
        private Rigidbody rb;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Reset()
        {
            InitialiseSettings();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        #region Editor Gizmos

        protected override void OnDrawGizmosConditional(bool selected)
        {
            Matrix4x4 resetMatrix = Gizmos.matrix;
            Gizmos.matrix = gameObject.transform.localToWorldMatrix;

            Gizmos.color = triggerColour;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.color = triggerOutlineColour;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = resetMatrix;
        }

        #endregion

#endif

        //----------------------------------------------------------------------------------------------------

        #region Functions

        private void InitialiseSettings()
        {
            SetupCollider();

            CameraMultiTarget cameraMultiTarget = GetComponent<CameraMultiTarget>();

            if (cameraMultiTarget != null)
            {
                triggerColour = new Color32(223, 128, 000, 079);
                triggerOutlineColour = new Color32(223, 128, 000, 128);
            }
        }

        private void SetupCollider()
        {
            if (gameObject.GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
            gameObject.GetComponent<Collider>().isTrigger = true;
        }

        public CameraRig GetCameraRig()
        {
            return cameraRig;
        }
        
        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getter / Setter

        public void SetCameraRig(CameraRig newCameraRig) { cameraRig = newCameraRig; }

        #endregion

    } //class end
}

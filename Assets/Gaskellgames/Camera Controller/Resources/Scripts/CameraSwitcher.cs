using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gaskellgames.CameraController
{
    public class CameraSwitcher : GGMonoBehaviour
    {
        #region Variables

        [Space, SerializeField, RequiredField]
        private CameraBrain cameraBrain;

        [SerializeField]
        [Tooltip("Use the global camera list")]
        private bool useRegisteredList = true;

        [LineSeparator, SerializeField]
        private InputActionProperty switchCamera; // ยังไม่ได้ใช้งาน

        [LineSeparator, SerializeField, RequiredField]
        private List<CameraRig> customCameraRigsList;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Reset / Debug [Editor]

#if UNITY_EDITOR

        private void Reset()
        {
            if (GetComponent<CameraBrain>() != null)
            {
                cameraBrain = GetComponent<CameraBrain>();
            }
        }

#endif

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game loop

        private void OnEnable()
        {
            // ลบการเชื่อมต่อกับ InputAction
            // switchCamera.action.performed += switchCameraCallback;
            // switchCamera.action.Enable();
        }

        private void OnDisable()
        {
            // switchCamera.action.performed -= switchCameraCallback;
            // switchCamera.action.Disable();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Input Action Callbacks

        private void switchCameraCallback(InputAction.CallbackContext context)
        {
            // ฟังก์ชันนี้จะไม่ใช้แล้ว
            // SwitchToNextCamera();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void SwitchToNextCamera()
        {
            if (cameraBrain != null)
            {
                List<CameraRig> registeredCameras = CameraList.GetCameraRigList();

                if (useRegisteredList && 1 < registeredCameras.Count)
                {
                    SelectNextCamera(registeredCameras);
                }
                else if (1 < customCameraRigsList.Count)
                {
                    SelectNextCamera(customCameraRigsList);
                }
            }
        }

        private void SelectNextCamera(List<CameraRig> cameraList)
        {
            CameraRig active = cameraBrain.GetActiveCamera();
            int activeIndex = -1;

            for (int i = 0; i < cameraList.Count; i++)
            {
                if (cameraList[i] == active)
                {
                    activeIndex = i;
                }
            }

            if (activeIndex != -1)
            {
                if (activeIndex == cameraList.Count - 1)
                {
                    activeIndex = 0;
                }
                else
                {
                    activeIndex++;
                }
                cameraBrain.SetActiveCamera(cameraList[activeIndex]);
            }
            else
            {
                cameraBrain.SetActiveCamera(cameraList[0]);
            }
        }

        // ฟังก์ชันใหม่ที่เปลี่ยนกล้องไปยังกล้องที่กำหนดเอง
        public void SwitchToCamera(int cameraIndex)
        {
            if (cameraBrain != null)
            {
                List<CameraRig> cameraList = useRegisteredList ? CameraList.GetCameraRigList() : customCameraRigsList;

                if (cameraIndex >= 0 && cameraIndex < cameraList.Count)
                {
                    cameraBrain.SetActiveCamera(cameraList[cameraIndex]);
                }
                else
                {
                    Debug.LogWarning("Camera index out of range.");
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getters / Setters

        public void ToggleNextCamera()
        {
            SwitchToNextCamera();
        }

        #endregion
    }
}

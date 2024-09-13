using UnityEngine;
using Gaskellgames.CameraController;

public class CameraControllerExample : MonoBehaviour
{
    [SerializeField]
    private CameraSwitcher cameraSwitcher;

    private void Update()
    {
        // กดปุ่ม '1' เพื่อเปลี่ยนไปยังกล้องที่ index 0
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraSwitcher.SwitchToCamera(0);
        }

        // กดปุ่ม '2' เพื่อเปลี่ยนไปยังกล้องที่ index 1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cameraSwitcher.SwitchToCamera(1);
        }

        // เพิ่มเงื่อนไขอื่นๆ ตามต้องการ
    }
}

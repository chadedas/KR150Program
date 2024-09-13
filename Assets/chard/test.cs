using System.Collections;
using System.Collections.Generic;
using Gaskellgames.CameraController;
using UnityEngine;

public class test : MonoBehaviour
{
    private CameraSwitcher cameraSwitcher;

    // Start is called before the first frame update
    void Start()
    {
        cameraSwitcher = GetComponent<CameraSwitcher>();
    }

    // Update is called once per frame
    void Update()
    {
        // ตรวจสอบให้แน่ใจว่ามีการตั้งค่า CameraSwitcher ไว้แล้ว
        if (cameraSwitcher == null)
        {
            Debug.LogWarning("CameraSwitcher is not assigned or found.");
            return;
        }

        // ตรวจสอบจำนวนกล้องที่มีอยู่
        List<CameraRig> cameraList = CameraList.GetCameraRigList(); // หรือใช้ customCameraRigsList ถ้าใช้
        if (cameraList.Count >= 2)
        {
            // ลองสลับกล้อง
            cameraSwitcher.SwitchToCamera(0);
            cameraSwitcher.SwitchToCamera(1);
        }
        else
        {
            Debug.LogWarning("Not enough cameras in the list.");
        }
    }
}

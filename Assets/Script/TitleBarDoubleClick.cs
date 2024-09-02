using UnityEngine;

public class TitleBarDoubleClick : MonoBehaviour
{
    private bool isDoubleClick = false;
    private float lastClickTime = 0f;
    private float catchTime = 0.25f; // ระยะเวลาที่นับว่าเป็น double-click

    void OnGUI()
    {
        // ตรวจสอบว่ามีการกดเมาส์ซ้ายหรือไม่
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            // ตรวจสอบว่าตำแหน่งของเมาส์อยู่ที่ Title Bar หรือไม่
            if (Event.current.mousePosition.y <= 30) // ปรับค่าความสูงของ Title Bar ที่นี่
            {
                // ตรวจสอบว่าคลิกที่สองเกิดขึ้นภายในช่วงเวลาที่กำหนดหรือไม่
                if (Time.time - lastClickTime < catchTime)
                {
                    isDoubleClick = true;
                }
                lastClickTime = Time.time;
            }
        }

        // หากตรวจพบ double-click
        if (isDoubleClick)
        {
            ToggleFullScreen();
            isDoubleClick = false;
        }
    }

    void ToggleFullScreen()
    {
        // ตรวจสอบสถานะเต็มหน้าจอและทำการสลับระหว่าง fullscreen กับ windowed mode
        if (Screen.fullScreen)
        {
            Screen.SetResolution(800, 600, false); // ปรับขนาดหน้าต่างได้ที่นี่
        }
        else
        {
            Screen.fullScreen = true;
        }
    }
}

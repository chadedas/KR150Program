using UnityEngine;
using UnityEngine.UI;

public class CursorChanger : MonoBehaviour
{
    // รูปภาพของ Cursor สำหรับแต่ละปุ่ม
    public Texture2D cursorA;
    public Texture2D cursorB;
    public Texture2D cursorC;

    // ปุ่มสำหรับเปลี่ยนรูปร่าง Cursor
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;

    private void Start()
    {
        // ตั้งค่าเริ่มต้นให้เป็นรูปร่าง A
        Cursor.SetCursor(cursorA, Vector2.zero, CursorMode.Auto);

        // เชื่อมต่อปุ่มกับฟังก์ชัน
        buttonA.onClick.AddListener(() => ChangeCursor(cursorA));
        buttonB.onClick.AddListener(() => ChangeCursor(cursorB));
        buttonC.onClick.AddListener(() => ChangeCursor(cursorC));
    }

    // ฟังก์ชันสำหรับเปลี่ยน Cursor
    private void ChangeCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}

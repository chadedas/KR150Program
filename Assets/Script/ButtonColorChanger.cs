using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    // ตัวแปรสำหรับปุ่ม
    public Button button1;
    public Button button2;
    public Button button3;

    // สีที่ใช้สำหรับการเปลี่ยนสี
    public Color color1 = Color.red;
    public Color color2 = Color.green;
    public Color color3 = Color.blue;

    // รูปภาพของ Cursor สำหรับแต่ละปุ่ม
    public Texture2D cursorTexture1;
    public Texture2D cursorTexture2;
    public Texture2D cursorTexture3;

    // สีเดิมของวัตถุและปุ่ม
    private Color originalColor1;
    private Color originalColor2;
    private Color originalColor3;
    private Color originalButtonColor1;
    private Color originalButtonColor2;
    private Color originalButtonColor3;
    private Color selectedButtonColor = new Color(196f / 255f, 196f / 255f, 196f / 255f);

    private Renderer currentObject;
    private Button currentButton;

    private void Start()
    {
        // เก็บสีเดิมของวัตถุ
        originalColor1 = GetObjectColorByTag("Object1");
        originalColor2 = GetObjectColorByTag("Object2");
        originalColor3 = GetObjectColorByTag("Object3");

        // เก็บสีเดิมของปุ่ม
        originalButtonColor1 = button1.image.color;
        originalButtonColor2 = button2.image.color;
        originalButtonColor3 = button3.image.color;

        // ตั้งค่าการตอบสนองของปุ่ม
        button1.onClick.AddListener(() => OnButtonClicked(button1, "Object1", color1, cursorTexture1));
        button2.onClick.AddListener(() => OnButtonClicked(button2, "Object2", color2, cursorTexture2));
        button3.onClick.AddListener(() => OnButtonClicked(button3, "Object3", color3, cursorTexture3));
    }

    private void OnButtonClicked(Button button, string tag, Color color, Texture2D cursorTexture)
    {
        Renderer obj = GetObjectByTag(tag);

        if (currentButton == button)
        {
            // ถ้ากดปุ่มเดียวกันซ้ำ
            if (currentObject != null)
            {
                ResetColor(currentObject);
            }

            // เปลี่ยนสีปุ่มกลับเป็นสีเดิม
            button.image.color = originalButtonColor1;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // รีเซ็ต Cursor
            currentObject = null; // ล้างสถานะของวัตถุที่เลือก
            currentButton = null; // ล้างสถานะของปุ่มที่เลือก
        }
        else
        {
            // ถ้ากดปุ่มใหม่
            if (currentObject != null)
            {
                ResetColor(currentObject);
            }

            if (currentButton != null)
            {
                // เปลี่ยนสีปุ่มก่อนหน้าเป็นสีเดิม
                currentButton.image.color = originalButtonColor1;
            }

            if (obj != null)
            {
                ChangeColor(obj, color);
                currentObject = obj;
            }

            // เปลี่ยนสีปุ่มปัจจุบันเป็นสีเทาและเปลี่ยน Cursor
            button.image.color = selectedButtonColor;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            currentButton = button;
        }
    }

    private Renderer GetObjectByTag(string tag)
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        return obj != null ? obj.GetComponent<Renderer>() : null;
    }

    private Color GetObjectColorByTag(string tag)
    {
        Renderer obj = GetObjectByTag(tag);
        return obj != null ? obj.material.color : Color.white;
    }

    private void ChangeColor(Renderer obj, Color color)
    {
        obj.material.color = color;
    }

    private void ResetColor(Renderer obj)
    {
        if (obj.CompareTag("Object1"))
        {
            obj.material.color = originalColor1;
        }
        else if (obj.CompareTag("Object2"))
        {
            obj.material.color = originalColor2;
        }
        else if (obj.CompareTag("Object3"))
        {
            obj.material.color = originalColor3;
        }
    }
}

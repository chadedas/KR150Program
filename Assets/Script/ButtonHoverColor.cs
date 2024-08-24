using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText; // Drag your Text component here
    public Color hoverColor = Color.red; // สีเมื่อ hover
    public Color clickColor = Color.black; // สีเมื่อกด
    private Color originalColor;

    void Start()
    {
        // เก็บสีเริ่มต้นของข้อความ
        originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // เปลี่ยนสีเมื่อ hover
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // เปลี่ยนกลับเป็นสีเดิมเมื่อเลื่อนเมาส์ออก
        buttonText.color = originalColor;
    }

    public void OnButtonClick()
    {
        // เปลี่ยนสีเมื่อกด
        buttonText.color = clickColor;
    }
}

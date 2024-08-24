using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage rawImage;  // ใส่ RawImage ที่ต้องการเปลี่ยน

    private Color originalColor;  // เก็บสีเดิมของ RawImage

    // กำหนดสีใหม่เมื่อ hover
    public Color hoverColor = Color.green;

    void Start()
    {
        if (rawImage != null)
        {
            originalColor = rawImage.color;  // เก็บสีเดิมของ RawImage
        }
    }

    // เมื่อ mouse pointer hover บนปุ่ม
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rawImage != null)
        {
            rawImage.color = hoverColor;
        }
    }

    // เมื่อ mouse pointer ออกจากปุ่ม
    public void OnPointerExit(PointerEventData eventData)
    {
        if (rawImage != null)
        {
            rawImage.color = originalColor;
        }
    }
}

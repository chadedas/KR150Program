using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public RectTransform windowRect; // กำหนด RectTransform ของหน้าต่าง
    private Vector2 originalSize;
    private Vector3 originalPosition;

    private void Start()
    {
        // บันทึกขนาดและตำแหน่งเดิม
        originalSize = windowRect.sizeDelta;
        originalPosition = windowRect.localPosition;
    }

    public void MinimizeWindow()
    {
        windowRect.gameObject.SetActive(false); // ซ่อนหน้าต่าง
    }

    public void MaximizeWindow()
    {
        windowRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        windowRect.localPosition = Vector3.zero;
    }

    public void RestoreWindow()
    {
        windowRect.sizeDelta = originalSize;
        windowRect.localPosition = originalPosition;
    }

    public void CloseWindow()
    {
        Destroy(windowRect.gameObject); // ปิด (ลบ) หน้าต่าง
    }
}

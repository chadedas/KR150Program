using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public RectTransform menuPanel; // อ้างอิงถึง Panel ที่ใช้เป็นเมนู
    public Button toggleButton; // อ้างอิงถึงปุ่มที่ใช้ในการย่อและขยายเมนู
    public Button closeButton; // อ้างอิงถึงปุ่มที่ใช้ในการปิดเมนู
    public GameObject scrollView; // อ้างอิงถึง Scroll View
    public Vector2 expandedSize = new Vector2(300, 500); // ขนาดเมนูเมื่อขยาย
    public Vector2 collapsedSize = new Vector2(0, 0); // ขนาดเมนูเมื่อย่อ
    private bool isExpanded = false;

    void Start()
    {
        // ตั้งค่าปุ่มให้เรียกฟังก์ชัน ToggleMenu เมื่อถูกคลิ๊ก
        toggleButton.onClick.AddListener(ToggleMenu);
        // ตั้งค่าปุ่มให้เรียกฟังก์ชัน CloseMenu เมื่อถูกคลิ๊ก
        closeButton.onClick.AddListener(CloseMenu);
        // ตั้งค่าขนาดเริ่มต้นของเมนูให้เป็นย่อ
        menuPanel.sizeDelta = collapsedSize;
        // ซ่อนปุ่ม CloseButton เริ่มต้น
        closeButton.gameObject.SetActive(false);
        // ซ่อน Scroll View เริ่มต้น
        scrollView.SetActive(false);
    }

    void ToggleMenu()
    {
        if (isExpanded)
        {
            // ย่อเมนู
            menuPanel.sizeDelta = collapsedSize;
            // แสดงปุ่ม ToggleButton
            toggleButton.gameObject.SetActive(true);
            // ซ่อนปุ่ม CloseButton
            closeButton.gameObject.SetActive(false);
            // ซ่อน Scroll View
            scrollView.SetActive(false);
        }
        else
        {
            // ขยายเมนู
            menuPanel.sizeDelta = expandedSize;
            // ซ่อนปุ่ม ToggleButton
            toggleButton.gameObject.SetActive(false);
            // แสดงปุ่ม CloseButton
            closeButton.gameObject.SetActive(true);
            // แสดง Scroll View
            scrollView.SetActive(true);
        }

        // สลับสถานะ
        isExpanded = !isExpanded;
    }

    void CloseMenu()
    {
        // ย่อเมนู
        menuPanel.sizeDelta = collapsedSize;
        // แสดงปุ่ม ToggleButton
        toggleButton.gameObject.SetActive(true);
        // ซ่อนปุ่ม CloseButton
        closeButton.gameObject.SetActive(false);
        // ซ่อน Scroll View
        scrollView.SetActive(false);
        // ตั้งค่าสถานะเป็นย่อ
        isExpanded = false;
    }
}

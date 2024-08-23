using UnityEngine;
using UnityEngine.UI;

public class MessageBoxController : MonoBehaviour
{
    public GameObject messagePanel;
    public Text messageText;

    void Start()
    {
        // ซ่อน MessagePanel ในตอนเริ่มต้น
        messagePanel.SetActive(false);
    }

    void Update()
    {
        // ตรวจสอบว่าคลิกเมาส์ซ้ายหรือไม่
        if (Input.GetMouseButtonDown(0))
        {
            ShowMessage("ข้อความที่ต้องการแสดง");
        }
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
    }

    public void HideMessage()
    {
        messagePanel.SetActive(false);
    }
}

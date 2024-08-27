using UnityEngine;
using UnityEngine.UI;

public class PasswordToggle : MonoBehaviour
{
    public InputField passwordField;
    public Button toggleButton;
    public Sprite showIcon;  // ไอคอนเมื่อแสดงรหัสผ่าน
    public Sprite hideIcon;  // ไอคอนเมื่อซ่อนรหัสผ่าน

    private bool isPasswordVisible = false;

    void Start()
    {
        // ตั้งค่าเริ่มต้นให้เป็นการซ่อนรหัสผ่าน
        UpdatePasswordField();
        toggleButton.onClick.AddListener(TogglePasswordVisibility);
    }

    void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;
        UpdatePasswordField();
    }

    void UpdatePasswordField()
    {
        if (isPasswordVisible)
        {
            passwordField.contentType = InputField.ContentType.Standard;  // แสดงรหัสผ่าน
            toggleButton.GetComponent<Image>().sprite = hideIcon;         // เปลี่ยนไอคอนเป็นไอคอนซ่อนรหัสผ่าน
        }
        else
        {
            passwordField.contentType = InputField.ContentType.Password;  // ซ่อนรหัสผ่าน
            toggleButton.GetComponent<Image>().sprite = showIcon;         // เปลี่ยนไอคอนเป็นไอคอนแสดงรหัสผ่าน
        }

        // อัพเดตข้อความใน InputField เพื่อให้การเปลี่ยนแปลงมีผล
        passwordField.ForceLabelUpdate();
    }
}

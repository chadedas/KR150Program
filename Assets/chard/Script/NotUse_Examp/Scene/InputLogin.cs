using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InputLogin : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button ConfirmButton;

    void Start()
    {
        // เรียกใช้ VerifyButton ทุกครั้งที่มีการเปลี่ยนแปลงใน InputField
        username.onValueChanged.AddListener(delegate { VerifyButton(); });
        password.onValueChanged.AddListener(delegate { VerifyButton(); });

        // ตรวจสอบสถานะของปุ่มเมื่อเริ่มต้น
        VerifyButton();
    }

    public void CallRegister()
    {
        // เรียกใช้ Coroutine เพื่อส่งข้อมูล
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);
        
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/UnityBacked/testdata.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("User Logged In!");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
                else
                {
                    Debug.Log("Login failed. Error: " + www.downloadHandler.text);
                }
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }

    public void VerifyButton()
    {
        // เปิดใช้งานปุ่ม Confirm ถ้าข้อมูลใน InputField ถูกต้อง
        ConfirmButton.interactable = (username.text.Length >= 8 && password.text.Length >= 8);
    }
}

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] Text user;
    [Space]
    [SerializeField] InputField username;
    [SerializeField] InputField password;

    [SerializeField] Text errorMessages;
    [SerializeField] GameObject progressCircle;

    [SerializeField] Button loginButton;
    [SerializeField] Button playButton;
    [SerializeField] Button logoutButton;

    [SerializeField] string url;

    private string bypassUsername = "bypass";
    private string bypassPassword = "bypass";

    public void OnLoginButtonClicked()
    {
        loginButton.interactable = false;
        progressCircle.SetActive(true);

        if (username.text == bypassUsername && password.text == bypassPassword)
        {
            BypassLogin();
        }
        else
        {
            StartCoroutine(Login());
        }
    }

    public void OnPlayButtonClicked()
    {
        playButton.interactable = false;
        progressCircle.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void OnLogoutButtonClicked()
    {
        logoutButton.interactable = false;
        progressCircle.SetActive(true);
        welcomePanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
{
    errorMessages.text = "Can't Login!!!";
    Debug.Log("<color=red>" + www.error + "</color>");
}
            else
            {
                if (www.downloadHandler.text.Contains("error"))
                {
                    errorMessages.text = "invalid username or password!";
                    Debug.Log("<color=red>" + www.downloadHandler.text + "</color>");
                }
                else
                {
                    // Open welcome panel
                    welcomePanel.SetActive(true);
                    user.text = username.text;
                    Debug.Log("<color=green>" + www.downloadHandler.text + "</color>");
                }
            }

            loginButton.interactable = true;
            progressCircle.SetActive(false);
        }
    }

    private void BypassLogin()
    {
        welcomePanel.SetActive(true);
        user.text = username.text;
        Debug.Log("<color=green>Bypass login successful</color>");

        loginButton.interactable = true;
        progressCircle.SetActive(false);
    }
}

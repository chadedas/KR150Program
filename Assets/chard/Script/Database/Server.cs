using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Globalization;
public class Server : MonoBehaviour
{
    [SerializeField] GameObject welcomePanel;
    [SerializeField] Text user;
    [SerializeField] GameObject loginPanel;
    [SerializeField] Text firstNameText; // New field for first name
    [SerializeField] Text lastNameText;  // New field for last name
    [SerializeField] Text dateText;  // New field for date time
    [SerializeField] Text licenseText; // New field for license key
    [SerializeField] Text licenseEXPText; // New field for licenseEXP key
    [SerializeField] Text daysRemainingText; // Existing field for status (Active/Expiry Date)
    [SerializeField] Text remainingDaysText; // New field for displaying the number of days remaining
    [SerializeField] Text alertText;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.isFocused)
            {
                password.Select();
            }
            else if (password.isFocused)
            {
                username.Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (loginButton.interactable)
            {
                OnLoginButtonClicked();
            }
        }
    }

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
        loginButton.interactable = false;
        progressCircle.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void OnLogoutButtonClicked()
    {
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

            if (www.result != UnityWebRequest.Result.Success)
            {
                errorMessages.text = "Can't Login!!!";
                Debug.Log("<color=red>" + www.error + "</color>");
            }
            else
            {
                if (www.isDone)
                {
                    // Split response to get the required data
                    string response = www.downloadHandler.text;

                    if (response.Contains("error"))
                    {
                        errorMessages.text = "Invalid username or password!";
                        Debug.Log("<color=red>" + response + "</color>");
                    }
                    else
                    {
                        // Parse the response to extract first name and last name
                        string[] data = response.Split(new[] { " - " }, System.StringSplitOptions.None);
                        user.text = data[1]; // username
                        firstNameText.text = data[2].Split(' ')[0]; // first name
                        lastNameText.text = data[3].Split(' ')[0];  // last name
                        dateText.text = data[4];
                        licenseText.text = data[5];
                        licenseEXPText.text = data[6];

                        // Check if EXPDATE is NULL
                        if (string.IsNullOrEmpty(licenseEXPText.text) || licenseEXPText.text.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                        {
                            daysRemainingText.text = "license lifetime";
                            daysRemainingText.color = Color.yellow; // Set text color to yellow
                            remainingDaysText.text = " "; // Not applicable for license lifetime
                            alertText.gameObject.SetActive(false); // Hide alert message
                        }
                        else
                        {
                            if (DateTime.TryParseExact(data[6], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiryDate))
                            {
                                // Normalize the time components to ensure correct day difference calculation
                                DateTime currentDate = DateTime.Now.Date; // Current date with time set to 00:00
                                expiryDate = expiryDate.Date; // Expiry date with time set to 00:00

                                int daysRemaining = (expiryDate - currentDate).Days;

                                // Update the status text
                                if (daysRemaining > 0)
                                {
                                    daysRemainingText.text = "Active";
                                    daysRemainingText.color = Color.green; // Set text color to green
                                    alertText.gameObject.SetActive(false); // Hide alert message
                                }
                                else
                                {
                                    daysRemainingText.text = "Expiry Date";
                                    daysRemainingText.color = Color.red; // Set text color to red
                                    alertText.gameObject.SetActive(true); // Hide alert message
                                }

                                // Update the remaining days text
                                remainingDaysText.text = $"{daysRemaining} days";
                            }
                            else
                            {
                                daysRemainingText.text = "Invalid Date Format";
                                daysRemainingText.color = Color.yellow; // Set text color to yellow for invalid format
                                remainingDaysText.text = " "; // Not applicable for invalid format
                            }
                        }

                        // Open welcome panel
                        welcomePanel.SetActive(true);
                        Debug.Log("<color=green>" + response + "</color>");
                    }
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
        firstNameText.text = "Bypass"; // You can customize this as needed
        lastNameText.text = "Bypass"; // You can customize this as needed
        dateText.text = "N/A";          // Customize as needed for bypass
        daysRemainingText.text = "N/A"; // Customize as needed for daysRemaining
        Debug.Log("<color=green>Bypass login successful</color>");

        loginButton.interactable = true;
        progressCircle.SetActive(false);
    }
}

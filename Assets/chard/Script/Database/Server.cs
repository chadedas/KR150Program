using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Net;

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
        StartCoroutine(Login());
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
    Dictionary<string, string> formData = new();
    formData["username"] = username.text;
    formData["password"] = password.text;

    using UnityWebRequest www = UnityWebRequest.Post("https://manage.np-robotics.com/api/index.php", formData);
    yield return www.SendWebRequest();

    if (www.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Error: " + www.error);
    }
    else
    {
        string response = www.downloadHandler.text;
        Debug.Log("Response: " + response);
        
if (response.StartsWith("ECHO DATABASE - "))
            {
                // Extract data from response
                string[] data = response.Split(new[] { " - " }, StringSplitOptions.None);

                if (data.Length >= 7)
                {
                    user.text = data[1]; // Username
                    firstNameText.text = data[2]; // First name
                    lastNameText.text = data[3];  // Last name
                    dateText.text = data[4]; // Permission 
                    if (data[5].Length == 16)
                    {
                        licenseText.text = string.Format("{0}-{1}-{2}-{3}",
                            data[5].Substring(0, 4),
                            data[5].Substring(4, 4),
                            data[5].Substring(8, 4),
                            data[5].Substring(12, 4));
                    }
                    else
                    {
                        Debug.LogError("License key does not have 16 characters.");
                    }

                    if (string.IsNullOrEmpty(data[6]))
                    {
                        data[6] = "ถาวร";
                        remainingDaysText.text = "ใช้งานได้";
                    }
                    else
                    {
                        // Convert license expiration date to DateTime
                        DateTime licenseExpiryDate;
                        if (DateTime.TryParseExact(licenseEXPText.text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out licenseExpiryDate))
                        {
                            // Get current date in Thailand timezone
                            TimeZoneInfo thailandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                            DateTime currentDateInThailand = TimeZoneInfo.ConvertTime(DateTime.Now, thailandTimeZone);

                            // Compare dates
                            if (DateTime.Compare(currentDateInThailand, licenseExpiryDate) < 0)
                            {
                                remainingDaysText.text = "ใช้งานได้";
                            }
                            else
                            {
                                remainingDaysText.text = "ไม่สามารถใช้งานได้";
                            }

                            // Display remaining days
                            TimeSpan remainingDays = licenseExpiryDate - currentDateInThailand;
                            daysRemainingText.text = remainingDays.Days.ToString();
                        }
                        else
                        {
                            Debug.LogError("รูปแบบวันหมดอายุไม่ถูกต้อง");
                        }
                    }
                    licenseEXPText.text = data[6];
                    // Update the UI
                    welcomePanel.SetActive(true);
                    loginPanel.SetActive(false);
                    Debug.Log("Login successful!");
                }
                else
                {
                    Debug.LogError("Response format is incorrect. Expected at least 8 parts.");
                    errorMessages.text = "Server response is incomplete.";
                }
            }
        else if (response.StartsWith("SERVER: error,"))
        {
            Debug.LogError("Invalid username or password.");
            errorMessages.text = "Invalid username or password.";
        }
        else
        {
            Debug.LogError("Unexpected response format: " + response);
        }
    }

    loginButton.interactable = true;
    progressCircle.SetActive(false);
}

}
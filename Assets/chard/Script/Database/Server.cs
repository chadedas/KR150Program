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

            if (data.Length >= 5)
            {
                // Assuming the response format: ECHO DATABASE - username - first name - last name - permission
                user.text = data[1]; // Username
                firstNameText.text = data[2]; // First name
                lastNameText.text = data[3];  // Last name
                dateText.text = data[4]; // Permission

                // Update the UI
                welcomePanel.SetActive(true);
                loginPanel.SetActive(false);
                Debug.Log("Login successful!");
            }
            else
            {
                Debug.LogError("Response format is incorrect. Expected at least 5 parts.");
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
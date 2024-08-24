using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    // Store the original resolution
    private int originalWidth;
    private int originalHeight;

    void Start()
    {
        // Save the current resolution
        originalWidth = Screen.width;
        originalHeight = Screen.height;

        // Set the screen to full-screen mode with the current display's resolution
        ToggleFullscreen();
    }

    void Update()
    {
        // Check if the player pressed F11
        if (Input.GetKeyDown(KeyCode.F11))
        {
            // Toggle the fullscreen mode
            ToggleFullscreen();
        }
    }

    void ToggleFullscreen()
    {
        if (!Screen.fullScreen)
        {
            // Get the current display's resolution
            Resolution[] resolutions = Screen.resolutions;
            Resolution maxResolution = resolutions[resolutions.Length - 1];

            // Set the screen to fullscreen with the maximum resolution
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);

            // Set the screen to fullscreen mode
            Screen.fullScreen = true;
        }
        else
        {
            // Exit fullscreen and restore the original resolution
            Screen.fullScreen = false;
            Screen.SetResolution(originalWidth, originalHeight, false);
        }
    }
}

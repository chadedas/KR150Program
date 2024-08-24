using UnityEngine;
using UnityEditor;

[InitializeOnLoad]  
public class FitWindowOnDoubleClick
{
    static FitWindowOnDoubleClick()
    {
        // Subscribe to the EditorApplication.update event to check for double clicks
        EditorApplication.update += CheckForDoubleClick;
    }

    private static bool isFirstClick = false;
    private static float firstClickTime;
    private const float doubleClickTimeLimit = 0.5f; // Time limit for double click

    private static void CheckForDoubleClick()
    {
        // Check if the editor window is in focus
        if (EditorWindow.focusedWindow == null) return;

        // Check for mouse click events
        if (Event.current != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (isFirstClick && (Time.time - firstClickTime < doubleClickTimeLimit))
            {
                // Double click detected
                FitEditorWindowToContent(EditorWindow.focusedWindow);
                isFirstClick = false;
            }
            else
            {
                // First click
                isFirstClick = true;
                firstClickTime = Time.time;
            }
        }
    }

    private static void FitEditorWindowToContent(EditorWindow window)
    {
        // Check if the window is an internal window
        if (window != null)
        {
            // Use reflection to access private fields/methods
            var type = window.GetType();
            var method = type.GetMethod("SetSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (method != null)
            {
                // Get content size
                var size = new Vector2(800, 600); // Default size, customize as needed
                method.Invoke(window, new object[] { size });
            }
        }
    }
}

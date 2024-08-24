using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeSceneToLogin()
    {
            SceneManager.LoadScene(2);
    }
    public void ChangeSceneToContact()
    {
            Debug.Log("ChangeSceneToContact");
    }
    public void ChangeSceneToExit()
    {
            Debug.Log("ChangeSceneToExit");
    }
}

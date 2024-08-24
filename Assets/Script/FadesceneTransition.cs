using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadesceneTransition : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // ตัว CanvasGroup ที่ใช้ในการ fade
    public float fadeDuration = 1.0f; // ระยะเวลาการ fade

    void Start()
    {
        if (fadeCanvasGroup != null)
        {
            // ตั้งค่าเริ่มต้นของความโปร่งใสของ CanvasGroup
            fadeCanvasGroup.alpha = 1;
            StartCoroutine(FadeOut());
        }
    }

    public void FadeAndLoadLevel(string sceneName)
    {
        StartCoroutine(FadeInAndLoad(sceneName));
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
    }

    IEnumerator FadeInAndLoad(string sceneName)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}

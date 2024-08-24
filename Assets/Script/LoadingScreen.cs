using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar; // ตัว Slider ที่จะใช้แสดงสถานะการโหลด
    public Text progressText; // ตัว Text ที่จะใช้แสดงสถานะการโหลด (ถ้ามี)
    public float fakeLoadingTime = 3.0f; // เวลาหน่วงในการโหลด (หน่วยเป็นวินาที)
    public CanvasGroup imageCanvasGroup; // ตัว CanvasGroup ของรูปภาพที่ต้องการให้ fade

    void Start()
    {
        // เรียก Coroutine เพื่อเริ่มโหลด Scene
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        // ตัวอย่างการโหลด Scene ที่ชื่อ "MainScene" แบบ Asynchronously
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("MainScene");
        
        // ปิดการโหลดอัตโนมัติเพื่อให้การโหลดช้าลง
        gameLevel.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < fakeLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            // อัพเดต Progress Bar และ Text
            progressBar.value = Mathf.Clamp01(elapsedTime / fakeLoadingTime);
            progressText.text = Mathf.RoundToInt(progressBar.value * 100) + "%";

            // ค่อยๆ เพิ่มค่าความโปร่งใสของรูปภาพ
            imageCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fakeLoadingTime);

            // รอให้ถึงเฟรมต่อไป
            yield return null;
        }

        // หลังจากเวลาหน่วงผ่านไป ให้โหลด Scene จริง
        progressBar.value = 1;
        progressText.text = "100%";
        imageCanvasGroup.alpha = 1;
        gameLevel.allowSceneActivation = true;
    }
}

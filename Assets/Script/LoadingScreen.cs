using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    public Text progressText;
    public CanvasGroup imageCanvasGroup;

    private float baseLoadingTime = 3.0f;
    private float adjustedLoadingTime;

    void Start()
    {
        adjustedLoadingTime = CalculateLoadingTime();
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("MainScene");
        gameLevel.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < adjustedLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / adjustedLoadingTime);
            progressText.text = Mathf.RoundToInt(progressBar.value * 100) + "%";
            imageCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / adjustedLoadingTime);

            yield return null;
        }

        progressBar.value = 1;
        progressText.text = "100%";
        imageCanvasGroup.alpha = 1;
        gameLevel.allowSceneActivation = true;
    }

    float CalculateLoadingTime()
    {
        // สเปคมาตรฐาน
        float baselineRAM = 8f; // GB
        float baselineVRAM = 4f; // GTX 1050 ~ 4 GB VRAM
        float baselineCPUSpeed = 3.0f; // GHz, ประมาณการสำหรับ i3-5400

        // สเปคระบบปัจจุบัน
        float currentRAM = SystemInfo.systemMemorySize / 1024f; // แปลงจาก MB เป็น GB
        float currentVRAM = SystemInfo.graphicsMemorySize / 1024f; // แปลงจาก MB เป็น GB
        float currentCPUSpeed = SystemInfo.processorFrequency / 1000f; // แปลงจาก MHz เป็น GHz

        // ปรับเวลาการโหลดตามสเปค
        float ramFactor = baselineRAM / currentRAM;
        float vramFactor = baselineVRAM / currentVRAM;
        float cpuFactor = baselineCPUSpeed / currentCPUSpeed;

        // เฉลี่ยปัจจัยเหล่านี้เพื่อหาค่าปรับสุดท้าย
        float performanceFactor = (ramFactor + vramFactor + cpuFactor) / 3f;

        // ให้แน่ใจว่าเวลาการโหลดไม่น้อยกว่าเกณฑ์ขั้นต่ำ (เช่น 1 วินาที)
        return Mathf.Max(baseLoadingTime * performanceFactor, 1.0f);
    }
}

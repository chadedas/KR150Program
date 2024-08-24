using UnityEngine;
using UnityEngine.UI;

public class CanvasResizer : MonoBehaviour
{
    public Canvas canvas;
    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        // ตั้งค่า Canvas Scaler
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0.5f;  // ปรับค่านี้ตามความเหมาะสม
    }
}

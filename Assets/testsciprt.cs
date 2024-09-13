using UnityEngine;

public class ClickAndDestroy : MonoBehaviour
{
    private Outline outline;
    protected bool isActive = false;

    protected virtual void Start()
    {
        // ดึง Component Outline จากวัตถุ
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            // ถ้าวัตถุไม่มี Outline Component ให้เพิ่ม
            outline = gameObject.AddComponent<Outline>();
        }
        // เริ่มต้นปิดการแสดง Outline
        outline.enabled = false;
    }
        public void Activate()
    {
        isActive = true;
        Debug.Log("Activated: " + gameObject.name);
    }



    private void OnMouseEnter()
    {
        // เปิดการแสดง Outline เมื่อเมาส์อยู่บนวัตถุ
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        // ปิดการแสดง Outline เมื่อเมาส์ออกจากวัตถุ
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}

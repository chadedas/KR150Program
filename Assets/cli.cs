 using System.Collections;
using UnityEngine;

public class ClickAndDestroy : MonoBehaviour
{
    public Animator animator;  // ตัวแปรที่จะเก็บ Animator ของวัตถุ
    public float delayBeforeDestroy = 5f;
    protected bool isActive = false;

    // ตัวแปรสำหรับการจัดการเอฟเฟกต์ Outline
    public Material outlineMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        objectRenderer = GetComponent<Renderer>();

        // เก็บ Material ดั้งเดิมของวัตถุ
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log("Activated: " + gameObject.name);
    }

    private void OnMouseEnter()
    {
        // เปลี่ยน Material เป็น Outline เมื่อเมาส์ชี้ไปที่วัตถุ
        if (objectRenderer != null && outlineMaterial != null)
        {
            objectRenderer.material = outlineMaterial;
        }
    }

    private void OnMouseExit()
    {
        // กลับไปใช้ Material ดั้งเดิมเมื่อเมาส์ออกจากวัตถุ
        if (objectRenderer != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }

    private void OnMouseDown()
    {
        animator.SetTrigger("A1");
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(delayBeforeDestroy);
        Destroy(gameObject);
    }
}

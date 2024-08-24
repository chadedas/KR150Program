using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 offset;
    public string destinationTag = "DropArea";
    private bool isDragging = false;

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
        transform.GetComponent<Collider>().enabled = false;
        isDragging = true; // เริ่มลาก
    }

    void OnMouseDrag()
    {
        if (isDragging) // อัปเดตตำแหน่งเฉพาะเมื่อกำลังลาก
        {
            transform.position = MouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
        {
            if (hitInfo.transform.CompareTag(destinationTag))
            {
                // ปรับตำแหน่งให้ตรงกับ DropArea
                transform.position = hitInfo.transform.position;

                // ตรงกับขนาดของ DropArea (ถ้าจำเป็น)
                transform.localScale = hitInfo.transform.localScale;
            }
        }

        // หยุดการลาก
        isDragging = false;
        transform.GetComponent<Collider>().enabled = true;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}

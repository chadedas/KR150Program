using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    public GameObject modelPrefab;

   public void OnDrop(PointerEventData eventData)
{
    Debug.Log("OnDrop called");

    // รับอ็อบเจ็กต์ที่ถูกลาก
    GameObject droppedObject = eventData.pointerDrag;

    if (droppedObject != null)
    {
        Debug.Log("Dropped object: " + droppedObject.name);

        // ตรวจสอบว่าอ็อบเจ็กต์ที่ถูกลากมีคอมโพเนนต์ DragHandler หรือไม่
        DragHandler dragHandler = droppedObject.GetComponent<DragHandler>();
        if (dragHandler != null && dragHandler.modelPrefab != null)
        {
            // ทำลายโมเดลเดิมถ้ามี
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            // สร้างโมเดลใหม่ที่พื้นที่วาง
            GameObject newModel = Instantiate(dragHandler.modelPrefab, transform.position, Quaternion.identity);

            // ตั้งค่าพ่อแม่ให้กับโมเดลใหม่
            newModel.transform.SetParent(this.transform);
        }
    }
}


}

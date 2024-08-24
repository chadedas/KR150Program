using UnityEngine;

public class ReplaceableModel : MonoBehaviour
{
    // สามารถใส่การตั้งค่าต่างๆ สำหรับโมเดลที่รอการแทนที่ได้ที่นี่

    // ฟังก์ชันนี้จะถูกเรียกเมื่อโมเดลถูกแทนที่
    public void OnReplaced(GameObject newModel)
    {
        // ทำการเปลี่ยนแปลงหรือจัดการหลังจากที่โมเดลถูกแทนที่
        Debug.Log($"{gameObject.name} has been replaced by {newModel.name}");
    }
}

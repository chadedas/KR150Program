using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ToggleButtons : MonoBehaviour, IPointerClickHandler
{
    public List<GameObject> buttons;
    public List<GameObject> elementsToMove;
    public float moveDistance = 30f; // ปรับความสูงที่ต้องการเลื่อน

    private bool areButtonsActive = false;

    void Start()
    {
        SetButtonsActive(false); // ซ่อนปุ่มตอนเริ่มเกม
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // สลับสถานะปุ่ม
        areButtonsActive = !areButtonsActive;
        SetButtonsActive(areButtonsActive);

        // เลื่อน UI อื่นๆ ขึ้นหรือลง
        foreach (GameObject element in elementsToMove)
        {
            RectTransform rectTransform = element.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float direction = areButtonsActive ? -moveDistance : moveDistance;
                rectTransform.anchoredPosition += new Vector2(0, direction);
            }
        }
    }

    private void SetButtonsActive(bool isActive)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(isActive);
        }
    }
}

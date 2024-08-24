using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu1; // Assign your first menu in the inspector
    public GameObject menu2; // Assign your second menu in the inspector

    private GameObject activeMenu;

    private void Start()
    {
        // Ensure both menus are inactive at the start
        menu1.SetActive(false);
        menu2.SetActive(false);
    }

    public void OpenMenu1()
    {
        // Close any active menu before opening Menu1
        CloseActiveMenu();
        menu1.SetActive(true);
        activeMenu = menu1;
    }

    public void OpenMenu2()
    {
        // Close any active menu before opening Menu2
        CloseActiveMenu();
        menu2.SetActive(true);
        activeMenu = menu2;
    }

    private void CloseActiveMenu()
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }
}

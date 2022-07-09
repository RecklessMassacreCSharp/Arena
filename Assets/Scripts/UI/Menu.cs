using UnityEngine;


public class Menu : MonoBehaviour
{
    private SetActive popupMenu;

    private void Start() {
        popupMenu = this.GetComponentInChildren<SetActive>();
        popupMenu.SetAct();      
    }

    private void ShowMenu() {
        popupMenu.SetAct();
    }
}

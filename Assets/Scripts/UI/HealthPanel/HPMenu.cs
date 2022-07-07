using UnityEngine;
using UnityEngine.EventSystems;


public class HPMenu : MonoBehaviour
{
    public RectTransform thisObjectRectTransform;
    private DragAndDrop dragAndDrop;

    private void Awake() {
        thisObjectRectTransform = transform as RectTransform;
        dragAndDrop = this.GetComponent<DragAndDrop>();
    }

    public void ChangeMovability() {
        dragAndDrop.movable = !dragAndDrop.movable; 
    }
}

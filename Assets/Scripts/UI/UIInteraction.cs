using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class UIInteraction : MonoBehaviour
{

    [SerializeField] private GameObject canvas;
    private GraphicRaycaster raycaster;

    private PointerEventData clickData;
    private List<RaycastResult> clickResults; 

    private GameObject[] interactableUIElements;

    void Awake() {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
    }


    void Update() {
        if (Mouse.current.rightButton.wasReleasedThisFrame) { 
            GetUIElementsClicked(); 
        }
    }

    private List<RaycastResult> GetUIElementsClicked() {
        clickData.position = Mouse.current.position.ReadValue();
        clickResults.Clear();

        raycaster.Raycast(clickData, clickResults);

        if (clickResults.Count > 0 && clickResults[0].gameObject.tag == "Interactable")
            clickResults[0].gameObject.SendMessage("ShowMenu");

        if (clickResults.Count > 0)
            return clickResults;
        
        return null;
    }
}

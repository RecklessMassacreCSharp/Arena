using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;


public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] float dampingSpeed = 0.05f;
    public bool movable = false;

    // For disablement camera movement when moving UI elements
    public bool isHandlingUI = false;
    [Tooltip("dynamic")] [SerializeField] private CinemachineVirtualCamera vcam;

    private RectTransform draggingObjectRectTransform;
    private Vector3 velocity;

    private void Awake() {
        draggingObjectRectTransform = transform as RectTransform;
    }

    public void OnDrag(PointerEventData eventData)
    {   
        if (movable)
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObjectRectTransform,
                eventData.position, eventData.pressEventCamera, out Vector3 globalMousePosition))
                    draggingObjectRectTransform.position = Vector3.SmoothDamp(draggingObjectRectTransform.position,
                    globalMousePosition, ref velocity, dampingSpeed);            
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (movable)
            vcam.gameObject.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        vcam.gameObject.SetActive(true);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForwardScrolling : MonoBehaviour, IScrollHandler
{
    [SerializeField] private GameObject targetObject;

    public void OnScroll(PointerEventData eventData)
    {
        // Check if the event data is null
        if (eventData == null)
        {
            Debug.LogWarning("PointerEventData is null");
            return;
        }

        // Check if the event data's pointer current raycast is null
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            Debug.LogWarning("PointerEventData's pointerCurrentRaycast is null");
            return;
        }

        // Forward the scroll event to the target object
        if(targetObject != null) ExecuteEvents.Execute(targetObject, eventData, ExecuteEvents.scrollHandler);
        else ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.scrollHandler);
    }
}

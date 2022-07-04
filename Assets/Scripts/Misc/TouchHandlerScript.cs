using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandlerScript : MonoBehaviour, IPointerDownHandler
{
    public string btName;

    public void OnPointerDown(PointerEventData eventData)
    {
        AllEventsScript.OnTouchCallback?.Invoke(btName);
    }
}

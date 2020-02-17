using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Go to " + item.url);
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        Application.OpenURL(item.url);
    }
}
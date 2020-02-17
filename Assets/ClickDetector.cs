using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerClickHandler
{
    public bool goToGabTrendsPage = false;
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        string url;
        if (goToGabTrendsPage)
            url = item.url;
        else
            url = item.id;
        Debug.Log("Go to " + url);
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        Application.OpenURL(url);
    }
}
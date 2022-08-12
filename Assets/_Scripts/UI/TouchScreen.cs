using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchScreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public UnityAction OnPointerUP;
    public UnityAction OnPointerClicked;
      
    private bool isClicked;

    public bool IsClicked
    {
        get { return isClicked; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        OnPointerClicked?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        OnPointerUP?.Invoke();
    }

}
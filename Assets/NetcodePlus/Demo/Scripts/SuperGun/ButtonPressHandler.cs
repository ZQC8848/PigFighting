using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isButtonPressed = false;

    // 当按钮被按下时调用
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
        //Debug.Log("Button is being pressed");
    }

    // 当按钮被释放时调用
    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
        //Debug.Log("Button is released");
    }
}

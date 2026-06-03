using UnityEngine;
using UnityEngine.EventSystems;

public class FrequencyButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    // Triple mini-game
    public TripleKnobMiniGame miniGame;

    public bool isPlusButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlusButton)
            miniGame.increase = true;
        else
            miniGame.decrease = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPlusButton)
            miniGame.increase = false;
        else
            miniGame.decrease = false;
    }
}
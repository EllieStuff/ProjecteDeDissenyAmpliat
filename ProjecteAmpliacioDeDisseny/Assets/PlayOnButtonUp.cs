using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayOnButtonUp : EventTrigger
{
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void PlayButtonUpSFX()
    {
        if(button.interactable)
            AudioManager.Play_SFX("ButtonUp_SFX");
        else
            AudioManager.Play_SFX("ButtonUpWrong_SFX", false);
    }




    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("The mouse click was released");
        //AudioManager.Play_SFX(soundName);
    }

}

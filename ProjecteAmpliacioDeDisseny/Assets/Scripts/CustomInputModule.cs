using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
    private bool
        touchIsPressed = false,
        touchIsReleased = false,
        mouseIsPressed = false;

    public bool IsPressedDown { get { return GetMousePointerEventData().AnyPressesThisFrame(); } }
    public bool IsPressed { get {
            if (Input.touchCount > 0)
                return touchIsPressed;
            else
                return mouseIsPressed;
        } 
    }
    public bool IsReleased { get { return touchIsReleased || GetMousePointerEventData().AnyReleasesThisFrame(); } }

    protected override void Start()
    {
        Input.simulateMouseWithTouches = true;
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    Vector2 tmpVec = new Vector2(Screen.width / 2, Screen.height / 2);
        //    ClickAt(tmpVec);
        //    //Debug.Log("click Pos: " + Camera.main.ScreenToWorldPoint(tmpVec));
        //}
        //Debug.Log("mouse Pos: " + Input.mousePosition);


        GetTouchPointerEventData(new Touch(), out touchIsPressed, out touchIsReleased);
        var mouseData = GetMousePointerEventData();
        if (mouseData.AnyPressesThisFrame()) mouseIsPressed = true;
        else if (mouseData.AnyReleasesThisFrame()) mouseIsPressed = false;

        //Debug.Log(IsReleased);
    }


    public void SetMouseState(Vector2 _pos, bool _pressed, bool _released)
    {
        //Input.simulateMouseWithTouches = true;
        var pointerData = GetTouchPointerEventData(new Touch()
        {
            position = new Vector2(_pos.x, _pos.y),
        }, out bool b, out bool bb);

        ProcessTouchPress(pointerData, _pressed, _released);
    }
    //public void ReleaseMouse(Vector2 _pos)
    //{
    //    //Input.simulateMouseWithTouches = true;
    //    var pointerData = GetTouchPointerEventData(new Touch()
    //    {
    //        position = new Vector2(_pos.x, _pos.y),
    //    }, out bool b, out bool bb);

    //    ProcessTouchPress(pointerData, false, true);
    //}

    //public void ClickAt(Vector2 _pos)
    //{
    //    Input.simulateMouseWithTouches = true;
    //    var pointerData = GetTouchPointerEventData(new Touch()
    //    {
    //        position = new Vector2(_pos.x, _pos.y),
    //    }, out bool b, out bool bb);

    //    ProcessTouchPress(pointerData, true, true);
    //}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesRecorder : MonoBehaviour
{
    public enum ButtonsState { NULL, FRONTWARD, BACKWARD }
    enum RecorderState { OFF, RECORDING, PLAYING }

    [SerializeField] RecorderState recorderState = RecorderState.OFF;
    [SerializeField] int targetFrameRate = 30;
    
    PlayerManagerScript playerScript;
    CustomInputModule inputModule;

    struct MouseData
    {
        public Vector2 position;
        public bool pressed, released;
        public MouseData(Vector2 _position, bool _pressed, bool _released) { position = _position; pressed = _pressed; released = _released; }
    }
    class SavedData
    {
        public MouseData mouseData;
        public Vector2 initForce;
        public SavedData(MouseData _mouseData, Vector2 _initForce)
            { mouseData = _mouseData; initForce = _initForce; }

        //public int 
        //    chooseItemId,
        //    initPosSliderInt = 0;
        //public Vector2
            //playerPos = Vector2.zero,
            //initForceSlider;
        //public ButtonsState buttonState;
        //public SavedData(int _chooseItemId, /*int _initPosSliderInt, Vector2 _initPosSlider,*/ Vector2 _initForceSlider, ButtonsState _buttonsState = ButtonsState.NULL) 
        //    { chooseItemId = _chooseItemId; /*initPosSliderInt = _initPosSliderInt; initPosSlider = _initPosSlider;*/ initForceSlider = _initForceSlider; buttonState = _buttonsState; }
    }
    List<SavedData> savedData = new List<SavedData>();


    public bool IsWorking { get { return recorderState != RecorderState.OFF; } }
    public bool IsRecording { get { return recorderState == RecorderState.RECORDING; } }
    public bool IsPlaying { get { return recorderState == RecorderState.PLAYING; } }
    public Vector2 CurrFrameInitForceSlider { get { return savedData[savedData.Count - 1].initForce; } }


    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        playerScript = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        Application.targetFrameRate = targetFrameRate;

        inputModule = GetComponent<CustomInputModule>();
        StartRecording();
    }


    private void Update()
    {
        switch (recorderState)
        {
            case RecorderState.OFF:
                // Do Nothing

                break;

            case RecorderState.RECORDING:
                savedData.Insert(0, new SavedData(
                        new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased),
                        new Vector2(playerScript.forceXSlider.value, playerScript.forceYSlider.value)
                    )
                );

                //savedData.Insert(0, new SavedData(
                //        playerScript.currItemId,
                //        //(int)playerScript.initialPosSlider.value,
                //        //new Vector2(playerScript.initialXSlider.value, playerScript.initialYSlider.value), 
                //        new Vector2(playerScript.forceXSlider.value, playerScript.forceYSlider.value)
                //    )
                //);

                //if (playerScript.useInitialPosSlider)
                //    savedData[0].initPosSliderInt = (int)playerScript.initialPosSlider.value;
                //else
                //    savedData[0].initPosSlider = new Vector2(playerScript.initialXSlider.value, playerScript.initialYSlider.value);

                //mouseData.Add(new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased));
                //if (framesRecorded < INT_CAPACITY)
                //    framesRecorded++;

                break;

            case RecorderState.PLAYING:
                if (savedData.Count > 0)
                {
                    int idx = savedData.Count - 1;
                    //playerScript.currItemId = savedData[idx].chooseItemId;
                    //if (playerScript.useInitialPosSlider) {
                    //    playerScript.initialPosSlider.value = savedData[idx].initPosSliderInt;
                    //} else {
                    //    playerScript.initialXSlider.value = savedData[idx].initPosSlider.x;
                    //    playerScript.initialYSlider.value = savedData[idx].initPosSlider.y;
                    //}
                    playerScript.forceXSlider.value = savedData[idx].initForce.x;
                    playerScript.forceYSlider.value = savedData[idx].initForce.y;
                    MouseData tmpMouseData = savedData[idx].mouseData;
                    inputModule.SetMouseState(tmpMouseData.position, tmpMouseData.pressed, tmpMouseData.released);
                    //playerScript.SetMoveDir(savedData[idx].movePointItem);
                    //ApplyFrameButtonStateEffect();

                    savedData.RemoveAt(idx);


                    //SavedData tmpMouseData = savedData[savedData.Count - 1];
                    //inputModule.SetMouseState(tmpMouseData.position, tmpMouseData.pressed, tmpMouseData.released);
                    //savedData.RemoveAt(savedData.Count - 1);
                }
                else
                {
                    StopPlaying();
                }

                //inputModule.enabled = true;
                //inputModule.SetMouseState(mouseData[framePlaying].position, mouseData[framePlaying].pressed, mouseData[framePlaying].released);
                //Debug.Log("mouseClicked: " + mouseData[framePlaying]);
                //if (framePlaying < framesRecorded - 1)
                //    framePlaying++;
                //else
                //    StopPlaying();

                //inputModule.enabled = false;


                break;


            default:
                break;
        }

    }


    [ContextMenu("StartRecording")]
    public void StartRecording()
    {
        recorderState = RecorderState.RECORDING;
    }
    [ContextMenu("StopRecording")]
    public void StopRecording()
    {
        recorderState = RecorderState.OFF;
    }
    [ContextMenu("EraseRecording")]
    public void EraseRecording()
    {
        //framesRecorded = framePlaying = 0;
        savedData.Clear();
        savedData = new List<SavedData>();
    }

    [ContextMenu("StartPlaying")]
    public void StartPlaying()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 0.5f;
        //inputModule.enabled = false;
        //framePlaying = 0;
        recorderState = RecorderState.PLAYING;
    }
    [ContextMenu("ReStartPlaying")]
    public void ReStartPlaying()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //inputModule.enabled = false;
        recorderState = RecorderState.PLAYING;
    }
    [ContextMenu("StopPlaying")]
    public void StopPlaying()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1.0f;
        //inputModule.enabled = true;
        recorderState = RecorderState.OFF;
    }


    public void SetFrameButtomState(ButtonsState _buttonsState)
    {
        //if (savedData.Count > 0)
        //    savedData[0].buttonState = _buttonsState;
    }
    private void ApplyFrameButtonStateEffect()
    {
        //switch(savedData[savedData.Count - 1].buttonState)
        //{
        //    case ButtonsState.NULL:
        //        // Do nothing

        //        break;

        //    case ButtonsState.FRONTWARD:
        //        playerScript.NextState();

        //        break;

        //    case ButtonsState.BACKWARD:
        //        playerScript.LastState();

        //        break;

        //    default:
        //        break;

        //}

    }


}

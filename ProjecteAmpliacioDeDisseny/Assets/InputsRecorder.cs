using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsRecorder : MonoBehaviour
{
    const int INT_CAPACITY = 2147483647;
    enum RecorderState { OFF, RECORDING, PLAYING }
    [SerializeField] RecorderState recorderState = RecorderState.OFF;

    CustomInputModule inputModule;
    
    struct MouseData
    {
        public Vector2 position;
        public bool pressed, released;
        public MouseData(Vector2 _position, bool _pressed, bool _released) { position = _position; pressed = _pressed; released = _released; }
    }
    List<MouseData> mouseData = new List<MouseData>();

    private int
        framesRecorded = 0,
        framePlaying = 0;

    public bool IsPlaying { get { return recorderState == RecorderState.PLAYING; } }
    public Vector2 CurrFrameMousePosition { get { return mouseData[framePlaying].position; } }
    public bool CurrFrameMousePressed { get { return mouseData[framePlaying].pressed; } }


    private void Start()
    {
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
                mouseData.Add(new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased));
                if (framesRecorded < INT_CAPACITY)
                    framesRecorded++;

                break;

            case RecorderState.PLAYING:
                //inputModule.enabled = true;
                inputModule.SetMouseState(mouseData[framePlaying].position, mouseData[framePlaying].pressed, mouseData[framePlaying].released);
                Debug.Log("mouseClicked: " + mouseData[framePlaying]);
                if (framePlaying < framesRecorded - 1)
                    framePlaying++;
                else
                    StopPlaying();

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
        framesRecorded = framePlaying = 0;
        mouseData = new List<MouseData>();
    }

    [ContextMenu("StartPlaying")]
    public void StartPlaying()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //inputModule.enabled = false;
        framePlaying = 0;
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
        //inputModule.enabled = true;
        recorderState = RecorderState.OFF;
    }

    public void EnableInputModule(bool _enable)
    {
        inputModule.enabled = _enable;
    }

}

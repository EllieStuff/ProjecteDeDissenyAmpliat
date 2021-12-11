using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsRecorder : MonoBehaviour
{
    const int INT_CAPACITY = 2147483647;
    enum RecorderState { OFF, RECORDING, PLAYING }
    [SerializeField] RecorderState recorderState = RecorderState.OFF;
    [SerializeField] int targetFrameRate = 30;

    CustomInputModule inputModule;
    
    struct MouseData
    {
        public Vector2 position;
        public bool pressed, released;
        public MouseData(Vector2 _position, bool _pressed, bool _released) { position = _position; pressed = _pressed; released = _released; }
    }
    List<MouseData> mouseData = new List<MouseData>();

    //private int
    //    framesRecorded = 0,
    //    framePlaying = 0;
    bool canUpdate = true;

    public bool IsPlaying { get { return recorderState == RecorderState.PLAYING; } }
    public Vector2 CurrFrameMousePosition { get { return mouseData[0].position; } }
    public bool CurrFrameMousePressed { get { return mouseData[0].pressed; } }


    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        inputModule = GetComponent<CustomInputModule>();
        StartRecording();
    }


    private void Update()
    {
        if (canUpdate)
        {
            switch (recorderState)
            {
                case RecorderState.OFF:
                    // Do Nothing

                    break;

                case RecorderState.RECORDING:
                    mouseData.Insert(0, new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased));

                    //mouseData.Add(new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased));
                    //if (framesRecorded < INT_CAPACITY)
                    //    framesRecorded++;

                    break;

                case RecorderState.PLAYING:
                    if (mouseData.Count > 0)
                    {
                        MouseData tmpMouseData = mouseData[mouseData.Count - 1];
                        inputModule.SetMouseState(tmpMouseData.position, tmpMouseData.pressed, tmpMouseData.released);
                        mouseData.RemoveAt(mouseData.Count - 1);
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

            //canUpdate = false;

        }

    }

    private void FixedUpdate()
    {
        if (recorderState != RecorderState.OFF)
            canUpdate = true;
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
        mouseData.Clear();
        mouseData = new List<MouseData>();
    }

    [ContextMenu("StartPlaying")]
    public void StartPlaying()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        //inputModule.enabled = true;
        recorderState = RecorderState.OFF;
    }

    public void EnableInputModule(bool _enable)
    {
        inputModule.enabled = _enable;
    }

}

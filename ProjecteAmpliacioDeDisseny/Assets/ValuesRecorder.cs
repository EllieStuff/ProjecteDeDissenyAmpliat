using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesRecorder : MonoBehaviour
{
    enum RecorderState { OFF, RECORDING, PLAYING }
    [SerializeField] RecorderState recorderState = RecorderState.OFF;
    [SerializeField] int targetFrameRate = 30;
    
    ThrowBallScript playerScript;

    struct SavedData
    {
        public Vector2
            initPosSlider,
            initForceSlider,
            moveDirBall;
        public SavedData(Vector2 _initPosSlider, Vector2 _initForceSlider, Vector2 _movePointBall) 
            { initPosSlider = _initPosSlider; initForceSlider = _initForceSlider; moveDirBall = _movePointBall; }
    }
    List<SavedData> savedData = new List<SavedData>();


    public bool IsPlaying { get { return recorderState == RecorderState.PLAYING; } }
    public Vector2 CurrFrameInitPosSlider { get { return savedData[savedData.Count - 1].initPosSlider; } }
    public Vector2 CurrFrameInitForceSlider { get { return savedData[savedData.Count - 1].initForceSlider; } }
    public Vector2 CurrFrameMovePointBall { get { return savedData[savedData.Count - 1].moveDirBall; } }


    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowBallScript>();

        StartRecording();
    }


    private void FixedUpdate()
    {
        switch (recorderState)
        {
            case RecorderState.OFF:
                // Do Nothing

                break;

            case RecorderState.RECORDING:
                savedData.Insert(0, new SavedData(
                        new Vector2(playerScript.initialXSlider.value, playerScript.initialYSlider.value),
                        new Vector2(playerScript.forceXSlider.value, playerScript.forceYSlider.value),
                        playerScript.GetMoveDir()
                    )
                );

                //mouseData.Add(new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased));
                //if (framesRecorded < INT_CAPACITY)
                //    framesRecorded++;

                break;

            case RecorderState.PLAYING:
                if (savedData.Count > 0)
                {
                    int idx = savedData.Count - 1;
                    playerScript.initialXSlider.value = savedData[idx].initPosSlider.x;
                    playerScript.initialYSlider.value = savedData[idx].initPosSlider.y;
                    playerScript.forceXSlider.value = savedData[idx].initForceSlider.x;
                    playerScript.forceYSlider.value = savedData[idx].initForceSlider.y;
                    playerScript.SetMoveDir(savedData[idx].moveDirBall);

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


}

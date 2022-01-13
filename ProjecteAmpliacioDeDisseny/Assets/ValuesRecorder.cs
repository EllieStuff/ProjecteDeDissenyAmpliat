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
    ChooseWeaponScript chooseItemsScript;
    bool[] lastChooseItemsTriggeringPlayer;
    Vector2[] initChooseItemsPos;

    class SavedData
    {
        public int
            chooseItemId,
            choosenItemId;
        public Vector2
            initPos = Vector2.zero,
            initForce;
        public Vector2[] chooseItems;
        public ButtonsState buttonState;
        public SavedData(int _chooseItemsId, int _choosenItemId, Vector2 _initPos, Vector2 _initForce, Vector2[] _chooseItems, ButtonsState _buttonsState = ButtonsState.NULL) 
            { chooseItemId = _chooseItemsId; choosenItemId = _choosenItemId; initPos = _initPos; initForce = _initForce; chooseItems = _chooseItems; buttonState = _buttonsState; }
    }
    List<SavedData> savedData = new List<SavedData>();


    public bool IsWorking { get { return recorderState != RecorderState.OFF; } }
    public bool IsRecording { get { return recorderState == RecorderState.RECORDING; } }
    public bool IsPlaying { get { return recorderState == RecorderState.PLAYING; } }
    public Vector2 CurrFrameInitPos { get { return savedData[savedData.Count - 1].initPos; } }
    public Vector2 CurrFrameInitForce { get { return savedData[savedData.Count - 1].initForce; } }


    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;

        playerScript = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();

        chooseItemsScript = GameObject.FindGameObjectWithTag("Weaponry").GetComponent<ChooseWeaponScript>();
        lastChooseItemsTriggeringPlayer = new bool[chooseItemsScript.WeaponsAmount];
        initChooseItemsPos = new Vector2[chooseItemsScript.WeaponsAmount];
        for(int i = 0; i < chooseItemsScript.WeaponsAmount; i++)
        {
            lastChooseItemsTriggeringPlayer[i] = false;
            initChooseItemsPos[i] = chooseItemsScript.WeaponsInitPos[i];
        }

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
                        chooseItemsScript.CurrUsedIdx, playerScript.currItemId,
                        playerScript.transform.position,
                        new Vector2(playerScript.forceXSlider.value, playerScript.forceYSlider.value),
                        chooseItemsScript.GetWeaponsCurrentPos()
                    )
                );

                //savedData[0].initPos = new Vector2(playerScript.initialXSlider.value, playerScript.initialYSlider.value);

                //mouseData.Add(new MouseData(Input.mousePosition, inputModule.IsPressed, inputModule.IsReleased));
                //if (framesRecorded < INT_CAPACITY)
                //    framesRecorded++;

                break;

            case RecorderState.PLAYING:
                // ToDo:
                //  - Adaptar les noves variables aqui
                //  - Fer que funcionin diferents les noves variables en els altres Scripts en "Playing"

                if (savedData.Count > 0)
                {
                    int idx = savedData.Count - 1;
                    playerScript.currItemId = savedData[idx].choosenItemId;

                    playerScript.initialXSlider.value = savedData[idx].initPos.x;
                    playerScript.initialYSlider.value = savedData[idx].initPos.y;

                    playerScript.forceXSlider.value = savedData[idx].initForce.x;
                    playerScript.forceYSlider.value = savedData[idx].initForce.y;
                    //playerScript.SetMoveDir(savedData[idx].movePointItem);
                    ApplyFrameButtonStateEffect();

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
        if (savedData.Count > 0)
            savedData[0].buttonState = _buttonsState;
    }
    private void ApplyFrameButtonStateEffect()
    {
        switch(savedData[savedData.Count - 1].buttonState)
        {
            case ButtonsState.NULL:
                // Do nothing

                break;

            case ButtonsState.FRONTWARD:
                playerScript.NextState();

                break;

            case ButtonsState.BACKWARD:
                playerScript.LastState();

                break;

            default:
                break;

        }

    }


}

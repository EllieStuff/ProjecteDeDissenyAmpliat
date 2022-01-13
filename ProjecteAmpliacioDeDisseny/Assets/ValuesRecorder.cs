using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesRecorder : MonoBehaviour
{
    public enum ButtonsState { NULL, FRONTWARD, BACKWARD }
    enum RecorderState { OFF, RECORDING, PLAYING }

    [SerializeField] RecorderState recorderState = RecorderState.OFF;
    [SerializeField] int targetFrameRate = 30;
    public float initReplayTimeScale = 0.5f;
    
    PlayerManagerScript playerScript;
    ChooseWeaponScript chooseItemsScript;
    //bool lastChooseItemsTouchingPlayer = false;
    //Vector2[] initChooseItemsPos;

    class SavedData
    {
        public int
            //chooseItemId,
            choosenItemId;
        public Vector2
            initPos = Vector2.zero,
            initForce;
        public Vector2[] chooseItems;
        public ButtonsState buttonState;
        public SavedData(/*int _chooseItemsId, */int _choosenItemId, Vector2 _initPos, Vector2 _initForce, Vector2[] _chooseItems, ButtonsState _buttonsState = ButtonsState.NULL) 
            { /*chooseItemId = _chooseItemsId; */choosenItemId = _choosenItemId; initPos = _initPos; initForce = _initForce; chooseItems = _chooseItems; buttonState = _buttonsState; }
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
        //initChooseItemsPos = new Vector2[chooseItemsScript.WeaponsAmount];
        //for(int i = 0; i < chooseItemsScript.WeaponsAmount; i++)
        //    initChooseItemsPos[i] = chooseItemsScript.WeaponsInitPos[i];

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
                        //chooseItemsScript.CurrUsedIdx, 
                        playerScript.currItemId,
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

                    playerScript.transform.position = savedData[idx].initPos;
                    //playerScript.initialXSlider.value = savedData[idx].initPos.x;
                    //playerScript.initialYSlider.value = savedData[idx].initPos.y;

                    playerScript.forceXSlider.value = savedData[idx].initForce.x;
                    playerScript.forceYSlider.value = savedData[idx].initForce.y;
                    //playerScript.SetMoveDir(savedData[idx].movePointItem);

                    if (playerScript.currState == PlayerManagerScript.State.EDITING_ITEM)
                    {
                        for (int i = 0; i < chooseItemsScript.WeaponsAmount; i++)
                        {
                            chooseItemsScript.WeaponList[i].position = savedData[idx].chooseItems[i];

                            if (savedData[idx].choosenItemId == i)
                            {
                                chooseItemsScript.WeaponList[i].gameObject.SetActive(false);
                            }
                            else
                            {
                                if (!chooseItemsScript.WeaponList[i].gameObject.activeSelf)
                                    chooseItemsScript.WeaponList[i].gameObject.SetActive(true);
                            }

                        }
                    }


                    // Creo que le he dado demasiadas vueltas y ni hace falta esto (???
                    //int currItemIdx = savedData[idx].chooseItemId;
                    //Vector2 currItemPos = chooseItemsScript.WeaponList[currItemIdx].position;
                    //if (currItemPos == initChooseItemsPos[currItemIdx] && lastChooseItemsTouchingPlayer) { }
                        
                    //initChooseItemsPos[currItemIdx] = chooseItemsScript.WeaponList[currItemIdx].position;
                    //lastChooseItemsTouchingPlayer = chooseItemsScript.TouchingPlayer;

                    ApplyFrameButtonStateEffect();

                    savedData.RemoveAt(idx);

                }
                else
                {
                    StopPlaying();
                }

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
        Time.timeScale = initReplayTimeScale;
        recorderState = RecorderState.PLAYING;
    }
    [ContextMenu("ReStartPlaying")]
    public void ReStartPlaying()
    {
        recorderState = RecorderState.PLAYING;
    }
    [ContextMenu("StopPlaying")]
    public void StopPlaying()
    {
        Time.timeScale = 1.0f;
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

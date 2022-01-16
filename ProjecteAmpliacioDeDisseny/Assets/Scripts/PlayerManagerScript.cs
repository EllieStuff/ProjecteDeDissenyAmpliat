using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagerScript : MonoBehaviour
{
    const float MINIMUM_FORCE = 1.0f;

    public enum State { DEFAULT = 0, EDITING_ITEM, EDITING_POS, EDITING_FORCE, THROWING, WAITING_FOR_THROW, THROW_DONE, COUNT }
    public State currState = State.DEFAULT;

    public Vector2 initialPosIncrease = Vector2.zero;
    public Vector2 initialForceIncrease = Vector2.zero;

    //public string[] itemsToChoose;
    //public Slider chooseItemSlider;
    public GameObject chooseWeaponGO;
    public ChoosePositionScript choosePositionScript;
    public Slider forceXSlider;
    public Slider forceYSlider;

    [SerializeField] GameObject replaySpeedButton;
    [SerializeField] ChangeStageButtonsManager changeStageButtons;

    ChooseWeaponScript chooseWeaponScript;
    CollectiblesManager collectiblesManager;
    DragPlayer dragScript;
    Transform throwItemsFather;
    ThrowItemScript currItem = null;
    public int currItemId = -1;
    TrajectoryCalculator trajectoryScript;
    ValuesRecorder recorder;
    Vector2 realInitPos;
    Quaternion realInitRot;
    
    Vector2 initPos;
    Vector2 initForce = Vector2.zero;
    //Vector2 moveDir = Vector2.zero;
    public Vector2 InitPos { get { return initPos; } }
    public Vector2 InitForce { get { return initForce; } }
    //public Vector2 MoveDir { get { return moveDir; } }
    public Vector2 CurrItemPos { get { return currItem.transform.position; } }
    public float CurrItemMass { get { return currItem.RB.mass; } }

    private bool repetitionPlayed = false;


    // Start is called before the first frame update
    void Start()
    {
        chooseWeaponScript = chooseWeaponGO.GetComponentInChildren<ChooseWeaponScript>();

        collectiblesManager = GameObject.FindGameObjectWithTag("CollectiblesManager").GetComponent<CollectiblesManager>();

        throwItemsFather = transform.GetChild(0);
        //currItem = throwItemsFather.GetChild(currItemId).GetComponent<ThrowItemScript>();

        dragScript = GetComponent<DragPlayer>();
        realInitPos = initPos = transform.position;
        realInitRot = transform.rotation;

        trajectoryScript = transform.GetChild(2).GetComponentInChildren<TrajectoryCalculator>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();

        //chooseItemSlider.maxValue = throwItemsFather.childCount - 1;
        //initialPosSlider.maxValue = initalPosArray.Length - 1;

        //InitForceArrows();

        //ChangeCurrState(State.EDITING_POS);
        //ChangeCurrState(State.EDITING_ITEM);
    }

    //private void InitForceArrows()
    //{
    //    forceArrowsFather = transform.GetChild(2).GetChild(0);
    //    forceArrows = new GameObject[forceArrowsFather.childCount];
    //    for (int i = 0; i < forceArrows.Length; i++)
    //        forceArrows[i] = forceArrowsFather.GetChild(i).gameObject;

    //    forceArrows[0].GetComponent<ForceArrowScript>().SetSlider(forceXSlider);
    //    forceArrows[1].GetComponent<ForceArrowScript>().SetSlider(forceYSlider);
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        StateMachine();

        //if ((int)currState >= (int)State.EDITING_FORCE)
        //    Debug.DrawRay(currItem.transform.position, moveDir, Color.red);

    }



    void StateMachine()
    {
        switch (currState)
        {
            case State.DEFAULT:
                //transform.position = initPos = GetInitialPos();

                NextState();

                break;

            case State.EDITING_ITEM:
                SetChosenItem();

                break;

            case State.EDITING_POS:
                //transform.position = initPos = GetInitialPos(); --> Ara el PlayerSet es mou al script DragPlayer
                trajectoryScript.transform.position = currItem.transform.position;

                break;

            case State.EDITING_FORCE:
                initForce = GetInitialForce();

                trajectoryScript.CalculateTrajectory(currItem.transform.position, initForce, currItem.RB.mass);

                break;

            //case State.EDITING_DIR:
            //    if (!recorder.IsPlaying)
            //    {
            //        if (Input.GetKey(KeyCode.Mouse0) /*|| recorder.CurrFrameMousePressed*/)
            //        {
            //            Ray ray;
            //            //if (recorder.IsPlaying) ray = Camera.main.ScreenPointToRay(recorder.CurrFrameMousePosition);
            //            //else ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //            RaycastHit hit;
            //            if (Physics.Raycast(ray, out hit))
            //            {
            //                if (!hit.transform.tag.Equals("NoClickAreas"))
            //                {
            //                    //if (recorder.IsPlaying) mousePos = Camera.main.ScreenToWorldPoint(recorder.CurrFrameMousePosition);
            //                    //else mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //                    moveDir = ((Vector2)currItem.transform.position - mousePos).normalized;
            //                }

            //            }
            //            else
            //            {
            //                //if (recorder.IsPlaying) mousePos = Camera.main.ScreenToWorldPoint(recorder.CurrFrameMousePosition);
            //                //else mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //                moveDir = ((Vector2)currItem.transform.position - mousePos).normalized;
            //            }

            //        }

            //    }
            //    else
            //    {
            //        //mousePos = Camera.main.ScreenToWorldPoint(recorder.CurrFrameMousePosition);
            //        //moveDir = ((Vector2)currItem.transform.position - mousePos).normalized;

            //        moveDir = recorder.CurrFrameMoveDirItem;
            //    }

                //trajectoryScript.CalculateTrajectory(currItem.transform.position, initForce, moveDir, currItem.RB.mass);

                //break;

            case State.THROWING:
                currItem.RB.isKinematic = false;
                currItem.RB.AddForce(initForce, ForceMode.Impulse);
                changeStageButtons.EnableButtons(false);

                NextState();

                break;

            case State.WAITING_FOR_THROW:
                // Interact On Collision

                break;

            case State.THROW_DONE:
                // ToDo: tema menus i esperar per animacions i stuff
                //currItem.RB.isKinematic = true;
                NextState();

                break;

            default:
                break;
        }

    }

    public void SetChosenItem()
    {
        if(currItemId != chooseWeaponScript.currUsedIdx)
        {
            if (!recorder.IsPlaying && currItemId < 0)
                changeStageButtons.EnableButtons(true);

            if (recorder.IsRecording)
                currItemId = chooseWeaponScript.currUsedIdx;
            else if (recorder.IsPlaying)
                chooseWeaponScript.currUsedIdx = currItemId;

            if (currItem != null)
                currItem.gameObject.SetActive(false);

            if (currItemId >= 0)
            {
                throwItemsFather.GetChild(currItemId).gameObject.SetActive(true);
                currItem = throwItemsFather.GetChild(currItemId).GetComponent<ThrowItemScript>();
            }

            //currItem.transform.position = initPos;

            //if (!recorder.IsPlaying)
            //    realInitRot = currItem.transform.rotation;
            //else
            //    currItem.transform.rotation = realInitRot;

        }


    }

    private Vector2 GetInitialForce()
    {
        return new Vector2(
            MINIMUM_FORCE + forceXSlider.value * initialForceIncrease.x,
            MINIMUM_FORCE + forceYSlider.value * initialForceIncrease.y
        );
    }

    //private void ForceArrowsSetActive(bool _activate)
    //{
    //    for (int i = 0; i < forceArrows.Length; i++)
    //        forceArrows[i].SetActive(_activate);
    //}

    private void EverythingSetActive(bool _activate)
    {
        // Choosing Item
        chooseWeaponGO.SetActive(_activate);

        // Editing Pos
        dragScript.enabled = _activate;
        choosePositionScript.SetCharacterPreviewsActive(_activate);

        // Editing Force
        forceXSlider.gameObject.SetActive(_activate);
        forceYSlider.gameObject.SetActive(_activate);
        //ForceArrowsSetActive(_activate);

        // Editin Dir
        trajectoryScript.gameObject.SetActive(_activate);

    }


    private void ChangeCurrState(State _newState)
    {
        CurrStateSetActive(false);
        currState = _newState;
        CurrStateSetActive(true);
    }

    private void CurrStateSetActive(bool _activate)
    {
        switch (currState)
        {
            case State.DEFAULT:
                EverythingSetActive(_activate);

                break;

            case State.EDITING_ITEM:
                chooseWeaponGO.SetActive(_activate);

                break;

            case State.EDITING_POS:
                dragScript.enabled = _activate;
                choosePositionScript.SetCharacterPreviewsActive(_activate);

                break;

            case State.EDITING_FORCE:
                forceXSlider.gameObject.SetActive(_activate);
                forceYSlider.gameObject.SetActive(_activate);
                //ForceArrowsSetActive(_activate);
                trajectoryScript.gameObject.SetActive(_activate);
                if (_activate)
                    trajectoryScript.SetData(currItem.GetComponent<MeshFilter>().mesh, currItem.GetComponent<MeshRenderer>().material, currItem.transform.localScale);

                break;

            //case State.EDITING_DIR:
            //    trajectoryScript.gameObject.SetActive(_activate);

            //    break;

            default:
                break;
        }
    }

    public void NextState()
    {
        if (currState < State.COUNT)
        {
            CurrStateSetActive(false);
            currState++;
            if (currState >= State.COUNT)
            {
                if (!repetitionPlayed)
                {
                    repetitionPlayed = true;
                    ReinitValues();
                    collectiblesManager.RestartCollectibles();
                    AudioManager.Play_OST("ThinkingMusic");
                    AudioManager.PlayRandomEinarRewindLine();
                    replaySpeedButton.SetActive(true);
                    currState = State.EDITING_ITEM;
                    forceXSlider.interactable = forceYSlider.interactable = false;
                    GameObject _sceneObjects = GameObject.Find("SceneObjects");
                    _sceneObjects.GetComponent<RestartSceneObjects>().manageGravity(false);
                    _sceneObjects.GetComponent<RestartSceneObjects>().RestoreSceneObjects();
                    GameObject.Find("Main Camera").GetComponent<VHSPostProcessEffect>().enabled = true;
                    recorder.StartPlaying();
                    _sceneObjects.GetComponent<RestartSceneObjects>().manageGravity(true);
                }
                else
                {
                    recorder.StopPlaying();
                    changeStageButtons.gameObject.SetActive(false);
                    GameObject.Find("Main Camera").GetComponent<VHSPostProcessEffect>().enabled = false;
                    GameObject.Find("Canvas").GetComponent<EndLevel>().StartEndLevelUI();
                }
            }
            CurrStateSetActive(true);


            if (recorder.IsRecording) 
                recorder.SetFrameButtomState(ValuesRecorder.ButtonsState.FRONTWARD);
        }

    }
    public void LastState()
    {
        if (currState > State.DEFAULT + 1)
        {
            CurrStateSetActive(false);
            currState--;
            CurrStateSetActive(true);

            if(recorder.IsRecording)
                recorder.SetFrameButtomState(ValuesRecorder.ButtonsState.BACKWARD);
        }

    }

    private void ReinitValues()
    {
        //EverythingSetActive(true);

        currItem.RB.isKinematic = true;
        transform.position = realInitPos;
        transform.rotation = realInitRot;
        currItemId = -1;

        EverythingSetActive(false);
    }


    //public Vector2 GetMoveDir()
    //{
    //    return moveDir;
    //}
    //public void SetMoveDir(Vector2 _moveDir)
    //{
    //    moveDir = _moveDir;
    //}

}

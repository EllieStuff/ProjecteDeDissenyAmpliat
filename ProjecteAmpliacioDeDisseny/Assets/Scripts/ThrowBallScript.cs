using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBallScript : MonoBehaviour
{
    const float MINIMUM_FORCE = 1.0f;

    public enum State { DEFAULT = 0, EDITING_ITEM, EDITING_POS, EDITING_FORCE, EDITING_DIR, THROWING, WAITING_FOR_THROW, THROW_DONE, COUNT }
    public State currState = State.DEFAULT;

    public Vector2 initialPosIncrease = Vector2.zero;
    public Vector2 initialForceIncrease = Vector2.zero;

    //public string[] itemsToChoose;
    public Slider chooseItemSlider;
    public bool useInitialPosSlider = false;
    public Vector2[] initalPosArray;
    public Slider initialPosSlider;
    public Slider initialXSlider;
    public Slider initialYSlider;
    public Slider forceXSlider;
    public Slider forceYSlider;

    Transform throwItemsFather;
    Transform currItemTransform;
    Rigidbody currItemRb;
    int currItemId;
    Transform forceArrowsFather;
    GameObject[] forceArrows;
    TrajectoryCalculator trajectoryScript;
    //InputsRecorder recorder;
    ValuesRecorder recorder;
    Vector2 realInitPos;
    Quaternion realInitRot;
    Vector2 initPos;
    Vector2 initForce = Vector2.zero;
    Vector2 moveDir = Vector2.zero;
    Vector2 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        throwItemsFather = transform.GetChild(0);
        currItemTransform = throwItemsFather.GetChild(0);
        currItemRb = currItemTransform.GetComponent<Rigidbody>();
        trajectoryScript = transform.GetChild(1).GetComponentInChildren<TrajectoryCalculator>();
        //recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<InputsRecorder>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();

        chooseItemSlider.maxValue = throwItemsFather.childCount - 1;
        initialPosSlider.maxValue = initalPosArray.Length - 1;

        realInitPos = transform.position;
        realInitRot = transform.rotation;

        InitForceArrows();
        //ChangeCurrState(State.EDITING_POS);
        ChangeCurrState(State.EDITING_ITEM);
    }

    private void InitForceArrows()
    {
        forceArrowsFather = transform.GetChild(1).GetChild(0);
        forceArrows = new GameObject[forceArrowsFather.childCount];
        for (int i = 0; i < forceArrows.Length; i++)
            forceArrows[i] = forceArrowsFather.GetChild(i).gameObject;

        forceArrows[0].GetComponent<ForceArrowScript>().SetSlider(forceXSlider);
        forceArrows[1].GetComponent<ForceArrowScript>().SetSlider(forceYSlider);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();

        if ((int)currState >= (int)State.EDITING_DIR)
            Debug.DrawRay(transform.position, moveDir, Color.red);

    }



    void StateMachine()
    {
        switch (currState)
        {
            case State.EDITING_ITEM:
                // ToDo
                SetChosenItem();

                // De momento...
                NextState();

                break;

            case State.EDITING_POS:
                transform.position = initPos = GetInitialPos();

                break;

            case State.EDITING_FORCE:
                initForce = GetInitialForce();

                break;

            case State.EDITING_DIR:
                if (!recorder.IsPlaying)
                {
                    if (Input.GetKey(KeyCode.Mouse0) /*|| recorder.CurrFrameMousePressed*/)
                    {
                        Ray ray;
                        //if (recorder.IsPlaying) ray = Camera.main.ScreenPointToRay(recorder.CurrFrameMousePosition);
                        //else ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (!hit.transform.tag.Equals("NoClickAreas"))
                            {
                                //if (recorder.IsPlaying) mousePos = Camera.main.ScreenToWorldPoint(recorder.CurrFrameMousePosition);
                                //else mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                                moveDir = ((Vector2)transform.position - mousePos).normalized;
                            }

                        }
                        else
                        {
                            //if (recorder.IsPlaying) mousePos = Camera.main.ScreenToWorldPoint(recorder.CurrFrameMousePosition);
                            //else mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                            moveDir = ((Vector2)transform.position - mousePos).normalized;
                        }

                    }

                }

                trajectoryScript.CalculateTrajectory(initPos, initForce, moveDir, currItemRb.mass);

                break;

            case State.THROWING:
                currItemRb.isKinematic = false;
                currItemRb.AddForce(moveDir * initForce, ForceMode.Impulse);
                ChangeCurrState(State.WAITING_FOR_THROW);

                break;

            case State.WAITING_FOR_THROW:
                // Interact On Collision

                break;

            case State.THROW_DONE:
                // ToDo: tema menus i esperar per animacions i stuff
                NextState();

                break;

            default:
                break;
        }

    }

    private void SetChosenItem()
    {
        // ToDo:
        //  - Activar un fill o un altre segons el valor del slider (agafats amb transfrom.GetChild(slider.value); )
        //  - Reasignar el rb perque agafi el del fill i es controli tot des del pare

    }

    Vector2 GetInitialPos()
    {
        if (useInitialPosSlider)
        {
            return initalPosArray[(int)initialPosSlider.value];
        }
        else
        {
            return new Vector2(
                realInitPos.x + initialXSlider.value * initialPosIncrease.x,
                realInitPos.y + initialYSlider.value * initialPosIncrease.y
            );
        }
    }

    private Vector2 GetInitialForce()
    {
        return new Vector2(
            MINIMUM_FORCE + forceXSlider.value * initialForceIncrease.x,
            MINIMUM_FORCE + forceYSlider.value * initialForceIncrease.y
        );
    }

    private void ForceArrowsSetActive(bool _activate)
    {
        for (int i = 0; i < forceArrows.Length; i++)
            forceArrows[i].SetActive(_activate);
    }

    private void EverythingSetActive(bool _activate)
    {
        // Choosing Item
        //chooseItemSlider.gameObject.SetActive(_activate);

        // Editing Pos
        //initialPosSlider.gameObject.SetActive(_activate);
        initialXSlider.gameObject.SetActive(_activate);
        initialYSlider.gameObject.SetActive(_activate);

        // Editing Force
        forceXSlider.gameObject.SetActive(_activate);
        forceYSlider.gameObject.SetActive(_activate);
        ForceArrowsSetActive(_activate);

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
                chooseItemSlider.gameObject.SetActive(_activate);

                break;

            case State.EDITING_POS:
                if (useInitialPosSlider)
                {
                    initialPosSlider.gameObject.SetActive(_activate);
                }
                else
                {
                    initialXSlider.gameObject.SetActive(_activate);
                    initialYSlider.gameObject.SetActive(_activate);
                }

                break;

            case State.EDITING_FORCE:
                forceXSlider.gameObject.SetActive(_activate);
                forceYSlider.gameObject.SetActive(_activate);
                ForceArrowsSetActive(_activate);

                break;

            case State.EDITING_DIR:
                trajectoryScript.gameObject.SetActive(_activate);

                break;

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
                ReinitValues();
                currState = State.EDITING_POS;

                recorder.StartPlaying();
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
        EverythingSetActive(true);

        currItemRb.isKinematic = true;
        transform.position = realInitPos;
        transform.rotation = realInitRot;

        EverythingSetActive(false);
    }


    public Vector2 GetMoveDir()
    {
        return moveDir;
    }
    public void SetMoveDir(Vector2 _moveDir)
    {
        moveDir = _moveDir;
    }


    //private void OnCollisionEnter(Collision col)
    //{
    //    if(currState == State.WAITING_FOR_THROW && (col.transform.CompareTag("Floor") || col.transform.CompareTag("Target")))
    //    {
    //        NextState();
    //    }

    //}

}

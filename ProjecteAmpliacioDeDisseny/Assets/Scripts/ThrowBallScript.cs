using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBallScript : MonoBehaviour
{
    const float MINIMUM_FORCE = 1.0f;

    public enum State { DEFAULT = 0, EDITING_POS, EDITING_FORCE, EDITING_DIR, THROWING, WAITING_FOR_THROW, THROW_DONE, COUNT }
    public State currState = State.DEFAULT;

    public Vector2 initialPosIncrease = Vector2.zero;
    public Vector2 initialForceIncrease = Vector2.zero;

    [SerializeField] Slider initialXSlider;
    [SerializeField] Slider initialYSlider;
    [SerializeField] Slider forceXSlider;
    [SerializeField] Slider forceYSlider;

    Rigidbody rb;
    GameObject[] forceArrows;
    TrajectoryCalculator trajectoryScript;
    Vector2 realInitPos;
    Vector2 initPos;
    Vector2 initForce = Vector2.zero;
    Vector2 moveDir = Vector2.zero;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trajectoryScript = GetComponentInChildren<TrajectoryCalculator>();
        
        realInitPos = transform.position;

        InitForceArrows();
        ChangeCurrState(State.EDITING_POS);
    }

    private void InitForceArrows()
    {
        Transform forceArrowsFather = transform.GetChild(0);
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
            case State.EDITING_POS:
                transform.position = initPos = GetInitialPos();

                break;

            case State.EDITING_FORCE:
                initForce = GetInitialForce();

                break;

            case State.EDITING_DIR:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (!hit.transform.tag.Equals("NoClickAreas"))
                        {
                            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            moveDir = ((Vector2)transform.position - mousePos).normalized;
                        }

                    }
                    else
                    {
                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        moveDir = ((Vector2)transform.position - mousePos).normalized;
                    }

                }

                trajectoryScript.CalculateTrajectory(initPos, initForce, moveDir, rb.mass);

                break;

            case State.THROWING:
                rb.isKinematic = false;
                rb.AddForce(moveDir * initForce, ForceMode.Impulse);
                ChangeCurrState(State.WAITING_FOR_THROW);

                break;

            case State.WAITING_FOR_THROW:


                break;

            default:
                break;
        }

    }

    Vector2 GetInitialPos()
    {
        return new Vector2(
            realInitPos.x + initialXSlider.value * initialPosIncrease.x,
            realInitPos.y + initialYSlider.value * initialPosIncrease.y
        );
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
        // Editing Pos
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

            case State.EDITING_POS:
                initialXSlider.gameObject.SetActive(_activate);
                initialYSlider.gameObject.SetActive(_activate);

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
        if (currState < State.COUNT - 1)
        {
            CurrStateSetActive(false);
            currState++;
            CurrStateSetActive(true);
        }

    }
    public void LastState()
    {
        if (currState > State.DEFAULT + 1)
        {
            CurrStateSetActive(false);
            currState--;
            CurrStateSetActive(true);
        }

    }

}

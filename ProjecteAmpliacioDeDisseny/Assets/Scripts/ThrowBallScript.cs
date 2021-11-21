using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBallScript : MonoBehaviour
{
    public enum State { EDITING, EDITING_POS = 0, EDITING_FORCE, EDITING_DIR, THROWING, WAITING_FOR_THROW, THROW_DONE }
    public State currState = State.EDITING;

    public float maxRange = 5.0f;
    public Vector2 initialPosIncrease = Vector2.zero;
    public Vector2 initialForceIncrease = Vector2.zero;

    [SerializeField] Slider initialX;
    [SerializeField] Slider initialY;
    [SerializeField] Slider forceX;
    [SerializeField] Slider forceY;

    Rigidbody rb;
    Vector2 realInitPos;
    Vector2 initPos;
    //float force = 5.0f;
    Vector2 initForce = Vector2.zero;
    Vector2 moveDir = Vector2.zero;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        realInitPos = transform.position;

        ChangeCurrState(State.EDITING_POS);
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
                transform.position = GetInitialPos();

                break;

            case State.EDITING_FORCE:
                initForce = GetInitialForce();

                break;

            case State.EDITING_DIR:
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetKey(KeyCode.Mouse0) && Vector3.Distance(mousePos, transform.position) < maxRange)
                {
                    Debug.Log("In");
                    moveDir = ((Vector2)transform.position - mousePos).normalized;
                }

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
            realInitPos.x + initialX.value * initialPosIncrease.x,
            realInitPos.y + initialY.value * initialPosIncrease.y
        );
    }

    private Vector2 GetInitialForce()
    {
        return new Vector2(
            forceX.value * initialForceIncrease.x,
            forceY.value * initialForceIncrease.y
        );
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
            case State.EDITING_POS:
                initialX.gameObject.SetActive(_activate);
                initialY.gameObject.SetActive(_activate);

                break;

            case State.EDITING_FORCE:
                forceX.gameObject.SetActive(_activate);
                forceY.gameObject.SetActive(_activate);

                break;

            case State.EDITING_DIR:


                break;

            default:
                break;
        }
    }

    public void NextState()
    {
        CurrStateSetActive(false);
        currState++;
        CurrStateSetActive(true);
    }

}

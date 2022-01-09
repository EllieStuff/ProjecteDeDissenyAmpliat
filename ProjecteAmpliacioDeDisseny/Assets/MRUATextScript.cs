using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MRUATextScript : MonoBehaviour
{
    private const string MRUA_DEFAULT_TEXT = "D.x = Do.x + Vo.x * t <br>" +
                                             "D.y = Do.y + Vo.y * t + 1/2 * g * t^2";
    
    private PlayerManagerScript playerManager;
    private ValuesRecorder recorder;
    private TextMeshProUGUI text;

    Vector2 initVel;
    float timePassed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();

        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //StateMachine();

        if (recorder.IsPlaying)
        {
            StateMachine();
        }
        else
        {

        }

    }


    private void StateMachine()
    {
        switch (playerManager.currState)
        {
            case PlayerManagerScript.State.EDITING_ITEM:
                text.text = MRUA_DEFAULT_TEXT;

                break;

            case PlayerManagerScript.State.EDITING_POS:
                text.text = "D.x = " + playerManager.InitPos.x.ToString("F2") + " + Vo.x * t <br>" +
                            "D.y = " + playerManager.InitPos.y.ToString("F2") + " + Vo.y * t + 1/2 * g * t^2";

                break;

            case PlayerManagerScript.State.EDITING_FORCE:
                Vector2 initVelProjected = playerManager.InitForce / playerManager.CurrItemMass;
                text.text = "D.x = " + playerManager.InitPos.x.ToString("F2") + " + " + initVelProjected.x.ToString("F2") + " * t <br>" +
                            "D.y = " + playerManager.InitPos.y.ToString("F2") + " + " + initVelProjected.y.ToString("F2") + " * t + 1/2 * g * t^2";

                break;

            //case PlayerManagerScript.State.EDITING_DIR:
            //    initVel = playerManager.InitForce * playerManager.MoveDir / playerManager.CurrItemMass;
            //    text.text = "D.x = " + playerManager.InitPos.x.ToString("F2") + " + " + initVel.x.ToString("F2") + " * t <br>" +
            //                "D.y = " + playerManager.InitPos.y.ToString("F2") + " + " + initVel.y.ToString("F2") + " * t + 1/2 * g * t^2";

            //    break;

            case PlayerManagerScript.State.THROWING:
                timePassed = 0.0f;

                break;

            case PlayerManagerScript.State.WAITING_FOR_THROW:
                text.text = playerManager.CurrItemPos.x.ToString("F2") + " = " + playerManager.InitPos.x.ToString("F2") + " + " + initVel.x.ToString("F2") + " * " + timePassed.ToString("F2") + " <br>" +
                            playerManager.CurrItemPos.y.ToString("F2") + " = " + playerManager.InitPos.y.ToString("F2") + " + " + initVel.y.ToString("F2") + " * " + timePassed.ToString("F2") + " + 1/2 * g * " + timePassed.ToString("F2") + "^2";

                timePassed += Time.deltaTime;

                break;

            case PlayerManagerScript.State.THROW_DONE:

                break;

            default:
                break;

        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MRUATextScript : MonoBehaviour
{
    private const string MRUA_DEFAULT_TEXT = "D.x = Do.x + Vo.x * T <br>" +
                                             "D.y = Do.y + Vo.y * T + 1/2 * G * T^2";
    
    private PlayerManagerScript playerManager;
    private ValuesRecorder recorder;
    private TextMeshProUGUI text;

    private Image hForce;
    private Image vForce;

    Vector2 initVel;
    float timePassed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();

        text = GetComponent<TextMeshProUGUI>();

        hForce = GameObject.Find("Horizontal Force").transform.GetChild(1).GetComponent<Image>();
        vForce = GameObject.Find("Vertical Force").transform.GetChild(1).GetComponent<Image>();
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
            updateGUIFormula();
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
                text.text = "D.x = " + playerManager.InitPos.x.ToString("F2") + " + Vo.x * T <br>" + System.Environment.NewLine + System.Environment.NewLine +
                            "D.y = " + playerManager.InitPos.y.ToString("F2") + " + Vo.y * T + 1/2 * G * T^2";

                break;

            case PlayerManagerScript.State.EDITING_FORCE:
                Vector2 initVelProjected = playerManager.InitForce / playerManager.CurrItemMass;
                text.text = "D.x = " + playerManager.InitPos.x.ToString("F2") + " + " + initVelProjected.x.ToString("F2") + " * T <br>" + System.Environment.NewLine + System.Environment.NewLine +
                            "D.y = " + playerManager.InitPos.y.ToString("F2") + " + " + initVelProjected.y.ToString("F2") + " * T + 1/2 * G * T^2";

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
                text.text = playerManager.CurrItemPos.x.ToString("F2") + " = " + playerManager.InitPos.x.ToString("F2") + " + " + initVel.x.ToString("F2") + " * " + timePassed.ToString("F2") + " <br>" + System.Environment.NewLine + System.Environment.NewLine +
                            playerManager.CurrItemPos.y.ToString("F2") + " = " + playerManager.InitPos.y.ToString("F2") + " + " + initVel.y.ToString("F2") + " * " + timePassed.ToString("F2") + " + 1/2 * 9.8 * " + timePassed.ToString("F2") + "^2";

                timePassed += Time.deltaTime;

                break;

            case PlayerManagerScript.State.THROW_DONE:
                hForce.transform.parent.gameObject.SetActive(false);
                vForce.transform.parent.gameObject.SetActive(false);
                break;

            default:
                break;

        }

    }

    public void updateGUIFormula()
    {
        text.text = "D.x = Do.x +        * T <br><br>" +
                    "D.y = Do.y +        * T + 1/2 * G * T^2";

        hForce.fillAmount = playerManager.forceXSlider.value;
        vForce.fillAmount = playerManager.forceYSlider.value;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MRUATextScript : MonoBehaviour
{
    private const string MRUA_DEFAULT_TEXT = "X = Xo + Vo.x * T <br><br>" +
                                             "Y = Yo + Vo.y * T + 1/2 * G * T^2";
    
    private PlayerManagerScript playerManager;
    private ValuesRecorder recorder;
    private TextMeshProUGUI text;

    private Image hForce;
    private Image vForce;

    private RectTransform hPos;
    private RectTransform vPos;

    private Vector2 minLine = new Vector2(-44, -42);
    private Vector2 maxLine = new Vector2(44, 42);


    Vector2 initVel;
    float timePassed = 0.0f;
    Vector2 finalPos;

    public Vector2 minScreen;
    public Vector2 maxScreen;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();

        text = GetComponent<TextMeshProUGUI>();

        hForce = GameObject.Find("Horizontal Force").transform.GetChild(1).GetComponent<Image>();
        vForce = GameObject.Find("Vertical Force").transform.GetChild(1).GetComponent<Image>();

        minScreen = GameObject.Find("Screen Maximums").transform.GetChild(0).transform.position;
        maxScreen = GameObject.Find("Screen Maximums").transform.GetChild(1).transform.position;

        hPos = GameObject.Find("Horizontal Position").transform.GetChild(0).GetComponent<RectTransform>();
        vPos = GameObject.Find("Vertical Position").transform.GetChild(0).GetComponent<RectTransform>();
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

                if (playerManager.isThrowDone)
                {
                    hForce.transform.parent.gameObject.SetActive(false);
                    vForce.transform.parent.gameObject.SetActive(false);

                    hPos.transform.parent.gameObject.SetActive(false);
                    vPos.transform.parent.gameObject.SetActive(false);

                    playerManager.isThrowDone = false;
                }

                break;

            case PlayerManagerScript.State.EDITING_POS:
                Vector2 finalInitPos = playerManager.CurrItemPos;
                text.text = "X = " + finalInitPos.x.ToString("F2") + " + Vo.x * T <br><br>" +
                            "Y = " + finalInitPos.y.ToString("F2") + " + Vo.y * T + 1/2 * G * T^2";

                

                break;

            case PlayerManagerScript.State.EDITING_FORCE:
                Vector2 initVelProjected = playerManager.InitForce / playerManager.CurrItemMass;
                finalPos = finalInitPos = playerManager.CurrItemPos;
                text.text = "X = " + finalInitPos.x.ToString("F2") + " + " + initVelProjected.x.ToString("F2") + " * T <br><br>" +
                            "Y = " + finalInitPos.y.ToString("F2") + " + " + initVelProjected.y.ToString("F2") + " * T + 1/2 * G * T^2";

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
                initVelProjected = playerManager.InitForce / playerManager.CurrItemMass;
                float resultX = finalPos.x + (Mathf.Round(initVelProjected.x * 100) / 100) * (Mathf.Round(timePassed * 100) / 100);
                float resultY = finalPos.y + (Mathf.Round(initVelProjected.y * 100) / 100) * (Mathf.Round(timePassed * 100) / 100) + (1/2) * 9.8f * Mathf.Pow((Mathf.Round(timePassed * 100) / 100), 2);
                text.text = resultX.ToString("F2") + " = " + finalPos.x.ToString("F2") + " + " + initVelProjected.x.ToString("F2") + " * " + (Mathf.Round(timePassed * 100) / 100).ToString("F2") + " <br><br>" +
                            resultY.ToString("F2") + " = " + finalPos.y.ToString("F2") + " + " + initVelProjected.y.ToString("F2") + " * " + (Mathf.Round(timePassed * 100) / 100).ToString("F2") + " + 1/2 * 9.8 * " + (Mathf.Round(timePassed * 100) / 100).ToString("F2") + "^2";

                timePassed += Time.deltaTime;

                break;

            case PlayerManagerScript.State.THROW_DONE:
                
                break;

            default:
                break;

        }

    }

    public void updateGUIFormula()
    {
        text.text = "X =        +        * T <br><br>" +
                    "Y =        +        * T + 1/2 * G * T^2";


        if (playerManager.currState != PlayerManagerScript.State.WAITING_FOR_THROW)
        {
            hForce.fillAmount = playerManager.forceXSlider.value;
            vForce.fillAmount = playerManager.forceYSlider.value;

            float lenScreenX = maxScreen.x - minScreen.x;
            float lenPScreenX = maxScreen.x - playerManager.CurrItemPos.x;

            float percentX = lenPScreenX * 100 / lenScreenX;

            float lenSquareX = maxLine.x - minLine.x;

            float posX = maxLine.x - (percentX * lenSquareX / 100);


            float lenScreenY = maxScreen.y - minScreen.y;
            float lenPScreenY = maxScreen.y - playerManager.CurrItemPos.y;

            float percentY = lenPScreenY * 100 / lenScreenY;

            float lenSquareY = maxLine.y - minLine.y;

            float posY = maxLine.y - (percentY * lenSquareY / 100);

            hPos.transform.localPosition = new Vector2(Mathf.Clamp(posX, minLine.x, maxLine.x), 0);
            vPos.transform.localPosition = new Vector2(0, Mathf.Clamp(posY, minLine.y, maxLine.y));
        }
        
    }

}

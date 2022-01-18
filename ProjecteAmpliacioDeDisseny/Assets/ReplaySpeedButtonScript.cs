using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplaySpeedButtonScript : MonoBehaviour
{
    const float MARGIN = 0.1f;

    [SerializeField] float[] timeSpeeds;
    [SerializeField] float lerpSpeed = 0.5f;
    [SerializeField] Transform lerpEditItem;
    [SerializeField] Transform lerpEditPos;
    [SerializeField] Transform lerpEditForce;
    [SerializeField] Transform lerpDefault;

    PlayerManagerScript playerScript;
    ValuesRecorder recorder;
    Transform changeSpeedButton, skipButton;
    TextMeshProUGUI buttonText;
    PlayerManagerScript.State savedState = PlayerManagerScript.State.DEFAULT;
    Vector2 lerpTarget;
    int speedIdx = 0;


    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();
        changeSpeedButton = transform.GetChild(0);
        buttonText = changeSpeedButton.GetComponentInChildren<TextMeshProUGUI>();
        skipButton = transform.GetChild(1);

        buttonText.text = "x" + timeSpeeds[speedIdx].ToString("F1");
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (savedState != playerScript.currState)
        {
            savedState = playerScript.currState;

            if (playerScript.currState == PlayerManagerScript.State.EDITING_ITEM) lerpTarget = lerpEditItem.position;
            else if (playerScript.currState == PlayerManagerScript.State.EDITING_POS) lerpTarget = lerpEditPos.position;
            else if (playerScript.currState == PlayerManagerScript.State.EDITING_FORCE) lerpTarget = lerpEditForce.position;
            else lerpTarget = lerpDefault.position;

        }

        if(Vector2.Distance(changeSpeedButton.position, lerpTarget) >= MARGIN)
        {
            changeSpeedButton.position = Vector2.Lerp(changeSpeedButton.position, lerpTarget, lerpSpeed * Time.deltaTime);
        }

    }


    public void ChangeSpeed()
    {
        speedIdx++;
        if (speedIdx >= timeSpeeds.Length) speedIdx = 0;
        Time.timeScale = timeSpeeds[speedIdx];
        buttonText.text = "x" + timeSpeeds[speedIdx].ToString("F1");
    }


    public void SkipButton()
    {
        recorder.StopPlaying();
        //Time.timeScale = 0;
        changeSpeedButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<VHSPostProcessEffect>().enabled = false;
        if (playerScript.tutorial == null)
            GameObject.Find("Canvas").GetComponent<EndLevel>().StartEndLevelUI();
        else
            playerScript.tutorial._tutoState = 4;
    }

}

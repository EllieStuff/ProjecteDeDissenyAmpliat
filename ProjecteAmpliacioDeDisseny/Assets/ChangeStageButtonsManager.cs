using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStageButtonsManager : MonoBehaviour
{
    [SerializeField] Button nextStageButton;
    [SerializeField] Button lastStageButton;


    private void Start()
    {
        EnableButtons(false);
    }

    public void EnableButtons(bool _enable)
    {
        lastStageButton.interactable = nextStageButton.interactable = _enable;
    }

}

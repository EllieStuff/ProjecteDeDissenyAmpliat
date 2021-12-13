using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevel : MonoBehaviour
{
    public GameObject _generalUI;

    public bool win;

    void Start()
    {
        _generalUI.SetActive(false);
    }

    public void StartEndLevelUI()
    {
        int index = 0;
        string message = "";

        if (win)
        {
            index = 1;
            message = "You win!!!";
        }
        else
        {
            index = 2;
            message = "Try again!!!";
        }

        _generalUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = message;
        _generalUI.transform.GetChild(0).GetChild(index).gameObject.SetActive(true);
        _generalUI.SetActive(true);
    }

    public void HasCollided()
    {
        win = true;
    }

    public void RetryLevel()
    { 
        //Recharge the Scene
    }
}

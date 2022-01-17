using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckValknautsOnLevel : MonoBehaviour
{
    const short FALSE = 0;
    const short TRUE = 1;

    [SerializeField] string levelName;
    [SerializeField] int valknautsTotalAmount = 2;

    TextMeshProUGUI text;
    string levelValknautsKey;
    int gottenValknauts = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        levelValknautsKey = levelName + "_Valknaut";
        for(int i = 0; i < valknautsTotalAmount; i++)
        {
            int hasValknaut = PlayerPrefs.GetInt(levelValknautsKey + i.ToString(), FALSE);
            if(hasValknaut == TRUE)
            {
                gottenValknauts++;
            }
        }

        text.text = gottenValknauts.ToString() + "/" + valknautsTotalAmount.ToString();
    }

}

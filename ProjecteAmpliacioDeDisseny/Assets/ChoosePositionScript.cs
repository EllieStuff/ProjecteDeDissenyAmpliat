using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePositionScript : MonoBehaviour
{
    PlayerManagerScript playerManager;
    GameObject[] characterPreviewsFromLocations;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        characterPreviewsFromLocations = new GameObject[transform.childCount];
        for(int i = 0; i < characterPreviewsFromLocations.Length; i++)
        {
            characterPreviewsFromLocations[i] = transform.GetChild(i).GetChild(0).gameObject;
            characterPreviewsFromLocations[i].SetActive(false);
        }

    }


    public void SetCharacterPreviewsActive(bool _activate)
    {
        for(int i = 0; i < characterPreviewsFromLocations.Length; i++)
        {
            characterPreviewsFromLocations[i].SetActive(_activate);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWeaponScript : MonoBehaviour
{
    [SerializeField] Slider slider;

    Transform weaponsModels, chosenWeaponSprite;
    int savedSliderValue = -1;


    // Start is called before the first frame update
    void Start()
    {
        weaponsModels = transform.GetChild(0);
        chosenWeaponSprite = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(savedSliderValue != (int)slider.value)
        {
            savedSliderValue = (int)slider.value;
            chosenWeaponSprite.position = weaponsModels.GetChild(savedSliderValue).position;
        }

    }

}

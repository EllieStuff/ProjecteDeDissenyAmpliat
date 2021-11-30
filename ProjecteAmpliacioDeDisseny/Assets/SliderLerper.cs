using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLerper : MonoBehaviour
{
    [SerializeField] float lerpSpeed = 8.0f;

    Slider slider;
    float initValue, currValue, targetValue;
    float timer = 0.0f;
    bool checkForChanges = true;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { SliderValueChange(); });
        initValue = currValue = targetValue = slider.value;
    }


    // Update is called once per frame
    void Update()
    {
        checkForChanges = false;
        
        if (slider.value != targetValue)
        {
            if (/*startLerp &&*/ timer < 1.0f)
            {
                currValue = Mathf.Lerp(initValue, targetValue, timer);
                timer += Time.deltaTime * lerpSpeed;
            }
            else
            {
                initValue = currValue = targetValue;
            }

            slider.value = currValue;

        }
        else
        {
            currValue = slider.value;
        }

        checkForChanges = true;
        
        
        //if(slider.value != initValue && slider.value != targetValue)
        //{
        //    //startLerp = true;
        //    timer = 0.0f;
        //    targetValue = slider.value;
        //}

    }

    private void SliderValueChange()
    {
        if (checkForChanges)
        {
            checkForChanges = false;
            Debug.Log("in");
            timer = 0.0f;
            targetValue = slider.value;
            slider.value = initValue = currValue;
        }
        else
        {
            checkForChanges = true;
        }

    }


}

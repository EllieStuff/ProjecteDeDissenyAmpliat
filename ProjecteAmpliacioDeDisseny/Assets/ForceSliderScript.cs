using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSliderScript : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] Image handle2Image;

    Slider slider;
    Color sliderFillColor;
    Color sliderHandle2Color;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        sliderFillColor = fillImage.color;
        sliderHandle2Color = handle2Image.color;
    }

    // Update is called once per frame
    void Update()
    {
        sliderFillColor.a = Mathf.Lerp(0.2f, 1.0f, slider.value);
        fillImage.color = sliderFillColor;

        sliderHandle2Color.a = Mathf.Lerp(0.2f, 1.0f, slider.value);
        handle2Image.color = sliderHandle2Color;
    }


}

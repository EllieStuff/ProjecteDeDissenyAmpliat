using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceArrowScript : MonoBehaviour
{
    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;

    Slider slider;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
            transform.localScale = Vector3.Lerp(minScale, maxScale, slider.value);
    }

    public void SetSlider(Slider _slider)
    {
        slider = _slider;
    }

}

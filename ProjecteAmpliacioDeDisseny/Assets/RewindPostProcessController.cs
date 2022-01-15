using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RewindPostProcessController : MonoBehaviour
{
    public float intensity;
    public float smoothness;

    private Animation volume;

    public bool playAnimation = false;

    void Start()
    {
        volume = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playAnimation)
        {
            volume.wrapMode = WrapMode.Once;
            volume.Play();
            playAnimation = false;
        }
    }
}

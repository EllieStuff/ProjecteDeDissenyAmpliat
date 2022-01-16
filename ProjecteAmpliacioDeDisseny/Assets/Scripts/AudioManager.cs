using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float SFX_Volume = 1.0f;
    [SerializeField] [Range(0, 1)] float OST_Volume = 1.0f;
    
    private static AudioSource SFX_AudioSource;
    private static AudioSource OST_AudioSource;
    private static ValuesRecorder recorder;


    public void Start()
    {
        SFX_AudioSource = transform.GetChild(0).GetComponent<AudioSource>();
        OST_AudioSource = transform.GetChild(1).GetComponent<AudioSource>();

        SFX_Volume = PlayerPrefs.GetFloat("sfxVolume", SFX_Volume);
        OST_Volume = PlayerPrefs.GetFloat("ostVolume", OST_Volume);
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();

        SFX_AudioSource.volume = SFX_Volume;
        OST_AudioSource.volume = OST_Volume;

        //StartCoroutine(PlayMusicTest());

    }


    public static void PlayRandomEinarGenericLine()
    {
        GameObject[] loadObjects = Resources.LoadAll<GameObject>("Audio/SFX/GenericEinarLines");
        if (loadObjects == null)
        {
            Debug.LogError("Audio from Audio/SFX/GenericEinarLines not found");
            return;
        }

        SoundData clipData = loadObjects[Random.Range(0, loadObjects.Length)].GetComponent<SoundData>();
        SFX_AudioSource.volume = clipData.volume;
        SFX_AudioSource.pitch = clipData.pitch;

        recorder.SetFrameSFX("GenericEinarLines/" + clipData.name);

        SFX_AudioSource.PlayOneShot(clipData.clip);
    }
    public static void PlayRandomEinarRewindLine()
    {
        GameObject[] loadObjects = Resources.LoadAll<GameObject>("Audio/SFX/RewindEinarLines");
        if (loadObjects == null)
        {
            Debug.LogError("Audio from Audio/SFX/RewindEinarLines not found");
            return;
        }

        SoundData clipData = loadObjects[Random.Range(0, loadObjects.Length)].GetComponent<SoundData>();
        SFX_AudioSource.volume = clipData.volume;
        SFX_AudioSource.pitch = clipData.pitch;

        recorder.SetFrameSFX("RewindEinarLines/" + clipData.name);

        SFX_AudioSource.PlayOneShot(clipData.clip);
    }

    public static void Play_SFX(string _audioName, bool _copyOnRecorder = true, float _customPitch = -1.0f)
    {
        GameObject loadObject = Resources.Load<GameObject>("Audio/SFX/" + _audioName);
        if (loadObject == null)
        {
            Debug.LogError("Audio " + _audioName + " not found");
            return;
        }

        SoundData clipData = loadObject.GetComponent<SoundData>();
        SFX_AudioSource.volume = clipData.volume;
        if (_customPitch <= 0) SFX_AudioSource.pitch = clipData.pitch;
        else SFX_AudioSource.pitch = _customPitch;

        if (_copyOnRecorder) 
            recorder.SetFrameSFX(_audioName);

        SFX_AudioSource.PlayOneShot(clipData.clip);
    }

    public static void Play_OST(string _audioName, bool _copyOnRecorder = true)
    {
        GameObject loadObject = Resources.Load<GameObject>("Audio/OST/" + _audioName);
        if (loadObject == null)
        {
            Debug.LogError("Audio " + _audioName + " not found");
            return;
        }

        SoundData clipData = loadObject.GetComponent<SoundData>();
        OST_AudioSource.volume = clipData.volume;
        OST_AudioSource.pitch = clipData.pitch;

        if (_copyOnRecorder)
            recorder.SetFrameOST(_audioName);

        OST_AudioSource.clip = clipData.clip;
        OST_AudioSource.Play();
    }


    public static bool OST_IsPlaying()
    {
        return OST_AudioSource.isPlaying;
    }


    //IEnumerator PlayMusicTest()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    Play_OST("ThinkingMusic");

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(5.0f);
    //        Play_SFX("coinSFX");
    //    }
    //}

}

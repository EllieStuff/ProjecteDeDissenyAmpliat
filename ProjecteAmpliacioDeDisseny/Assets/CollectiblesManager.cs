using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectiblesManager : MonoBehaviour
{
    const short FALSE = 0;
    const short TRUE = 1;

    [SerializeField] float obtainedValknutAlpha = 0.3f;

    internal PlayerManagerScript playerManager;
    internal ValuesRecorder recorder;

    ValknutScript[] valknuts;
    List<int> valknautsAdded = new List<int>();
    string levelValknautsKey;


    private void Awake()
    {
        valknuts = new ValknutScript[transform.childCount];
        for (int i = 0; i < valknuts.Length; i++)
        {
            valknuts[i] = transform.GetChild(i).GetComponent<ValknutScript>();
            valknuts[i].id = i;
        }
    }

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        recorder = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ValuesRecorder>();
        levelValknautsKey = SceneManager.GetActiveScene().name + "_Valknaut";

        LoadValknauts();
    }


    internal void AddValknaut(int _valknautId)
    {
        valknautsAdded.Add(_valknautId);
    }

    public void LoadValknauts()
    {
        for (int i = 0; i < valknuts.Length; i++)
        {
            int valknautGot = PlayerPrefs.GetInt(levelValknautsKey + i.ToString(), FALSE);
            if (valknautGot == TRUE)
            {
                valknuts[i].SetTextureAlpha(obtainedValknutAlpha);
            }
            else
            {
                valknuts[i].SetTextureAlpha(1.0f);
            }
        }

    }
    public void SaveValknauts()
    {
        for(int i = 0; i < valknautsAdded.Count; i++)
        {
            PlayerPrefs.SetInt(levelValknautsKey + valknautsAdded[i].ToString(), TRUE);
        }

    }

    public void RestartCollectibles()
    {
        for(int i = 0; i < valknuts.Length; i++)
        {
            valknuts[i].gameObject.SetActive(true);
            valknuts[i].meshRenderer.material.color = valknuts[i].initColor;
        }

    }


    [ContextMenu("ResetSavedValues")]
    public void ResetSavedValues()
    {
        for(int i = 0; i < valknuts.Length; i++)
        {
            PlayerPrefs.SetInt(levelValknautsKey + i.ToString(), FALSE);
        }
    }

}

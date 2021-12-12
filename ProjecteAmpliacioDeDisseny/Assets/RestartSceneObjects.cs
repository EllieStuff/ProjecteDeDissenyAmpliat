using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSceneObjects : MonoBehaviour
{
    public Vector3[] _sceneObjectsPos;
    public Quaternion[] _sceneObjectsRot;
    
    int childCount = 0;

    void Start()
    {
        childCount = this.transform.childCount;
        _sceneObjectsPos = new Vector3[childCount];
        _sceneObjectsRot = new Quaternion[childCount];

        for (int i = 0; i < childCount; i++)
        {
            _sceneObjectsPos[i] = this.transform.GetChild(i).transform.position;
            _sceneObjectsRot[i] = this.transform.GetChild(i).transform.rotation;
        }
    }

    public void restoreSceneObjects()
    {      
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).transform.position = _sceneObjectsPos[i];
            this.transform.GetChild(i).transform.rotation = _sceneObjectsRot[i];
        }
        Debug.Log("Restored");
    }
}

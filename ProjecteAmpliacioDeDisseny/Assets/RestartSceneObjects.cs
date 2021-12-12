using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSceneObjects : MonoBehaviour
{
    [SerializeField] Vector3[] _sceneObjectsPos;
    [SerializeField] Quaternion[] _sceneObjectsRot;
    
    int childCount = 0;

    void Start()
    {
        childCount = this.transform.childCount;
        _sceneObjectsPos = new Vector3[childCount];
        _sceneObjectsRot = new Quaternion[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            _sceneObjectsPos[i] = child.position;
            _sceneObjectsRot[i] = child.rotation;
        }
    }

    public void RestoreSceneObjects()
    {      
        for (int i = 0; i < childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.position = _sceneObjectsPos[i];
            child.rotation = _sceneObjectsRot[i];
            Rigidbody childRb = child.GetComponent<Rigidbody>();
            childRb.velocity = childRb.angularVelocity = Vector3.zero;
        }
        Debug.Log("Restored");
    }
}

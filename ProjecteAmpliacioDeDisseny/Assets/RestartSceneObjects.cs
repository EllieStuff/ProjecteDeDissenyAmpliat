using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSceneObjects : MonoBehaviour
{
    public Vector3[] _throwObjectsPos;
    public Quaternion[] _throwObjectsRot;

    public Vector3[] _sceneObjectsPos;
    public Quaternion[] _sceneObjectsRot;
    
    int throwChildCount = 0;
    int sObjectsChildCount = 0;

    private GameObject throwParentGameObject;

    void Start()
    {
        sObjectsChildCount = this.transform.childCount;
        _sceneObjectsPos = new Vector3[sObjectsChildCount];
        _sceneObjectsRot = new Quaternion[sObjectsChildCount];

        for (int i = 0; i < sObjectsChildCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            _sceneObjectsPos[i] = child.position;
            _sceneObjectsRot[i] = child.rotation;
        }

        throwParentGameObject = GameObject.Find("ThrowItems");

        throwChildCount = throwParentGameObject.transform.childCount;
        _throwObjectsPos = new Vector3[throwChildCount];
        _throwObjectsRot = new Quaternion[throwChildCount];

        for (int i = 0; i < throwChildCount; i++)
        {
            _throwObjectsPos[i] = throwParentGameObject.transform.GetChild(i).transform.position;
            _throwObjectsRot[i] = throwParentGameObject.transform.GetChild(i).transform.localRotation;
        }
    }

    public void RestoreSceneObjects()
    {      
        for (int i = 0; i < sObjectsChildCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            child.position = _sceneObjectsPos[i];
            child.rotation = _sceneObjectsRot[i];
            Rigidbody childRb = child.GetComponent<Rigidbody>();
            childRb.velocity = childRb.angularVelocity = Vector3.zero;
        }

        for (int i = 0; i < throwChildCount; i++)
        {
            throwParentGameObject.transform.GetChild(i).transform.position = _throwObjectsPos[i];
            throwParentGameObject.transform.GetChild(i).transform.localRotation = _throwObjectsRot[i];
        }
    }

    public void manageGravity(bool isActive)
    {
        for (int i = 0; i < sObjectsChildCount; i++)
        {
            this.transform.GetChild(i).GetComponent<Rigidbody>().useGravity = isActive;
            this.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = !isActive;
        }
    }
}

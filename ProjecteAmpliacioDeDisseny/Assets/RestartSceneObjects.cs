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
            _sceneObjectsPos[i] = this.transform.GetChild(i).transform.position;
            _sceneObjectsRot[i] = this.transform.GetChild(i).transform.rotation;
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

    public void restoreSceneObjects()
    {      
        for (int i = 0; i < sObjectsChildCount; i++)
        {
            this.transform.GetChild(i).transform.position = _sceneObjectsPos[i];
            this.transform.GetChild(i).transform.rotation = _sceneObjectsRot[i];
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

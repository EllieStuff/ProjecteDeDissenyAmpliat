using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemScript : MonoBehaviour
{
    private ThrowBallScript manager;
    private Rigidbody rb;
    public Rigidbody RB { get { return rb; } }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<ThrowBallScript>();
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision col)
    {
        if (manager.currState == ThrowBallScript.State.WAITING_FOR_THROW && (col.transform.CompareTag("Floor") || col.transform.CompareTag("Target")))
        {
            manager.NextState();
        }

    }

}

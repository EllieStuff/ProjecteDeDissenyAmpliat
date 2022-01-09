using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemScript : MonoBehaviour
{
    private PlayerManagerScript manager;
    private Rigidbody rb;
    private Collider col;
    private PlayerManagerScript.State playerState = PlayerManagerScript.State.DEFAULT;

    public Rigidbody RB { get { return rb; } }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    private void Update()
    {
        if(playerState != manager.currState)
        {
            playerState = manager.currState;
            if(playerState == PlayerManagerScript.State.THROWING)
            {
                col.enabled = true;
            }
        }

    }


    private void OnCollisionEnter(Collision col)
    {
        if (manager.currState == PlayerManagerScript.State.WAITING_FOR_THROW && (col.transform.CompareTag("Floor") || col.transform.CompareTag("Target")))
        {
            manager.NextState();
        }

    }

}

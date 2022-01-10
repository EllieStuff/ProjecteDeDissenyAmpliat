using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemScript : MonoBehaviour
{
    private PlayerManagerScript manager;
    private Rigidbody rb;
    private Collider collider;
    Transform targetTransform;
    private bool timeOfGrace = true;
    private Vector2 lastPos = Vector2.zero;
    private bool firstIt = true;
    //private PlayerManagerScript.State playerState = PlayerManagerScript.State.DEFAULT;

    public Rigidbody RB { get { return rb; } }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        targetTransform = GameObject.FindGameObjectWithTag("Target").transform;
        collider.enabled = false;
    }

    private void Update()
    {
        if (manager.currState == PlayerManagerScript.State.THROWING)
        {
            collider.enabled = true;
            StartCoroutine(TimeOfGrace());
        }
        else if (manager.currState == PlayerManagerScript.State.WAITING_FOR_THROW)
        {
            float
                lastPos2TargetDistance = Vector2.Distance(lastPos, targetTransform.position),
                currPos2TargetDistance = Vector2.Distance(transform.position, targetTransform.position);
            if (!timeOfGrace && (rb.velocity == Vector3.zero || lastPos2TargetDistance < currPos2TargetDistance))
                FinishState();

            lastPos = transform.position;

            Debug.Log("LastPos Distance: " + lastPos2TargetDistance);
            Debug.Log("CurrPos Distance: " + currPos2TargetDistance);
        }

    }


    private void FinishState()
    {
        if (firstIt)
        {
            firstIt = false;
            collider.enabled = false;
            timeOfGrace = true;
            lastPos = Vector2.zero;

        }
        manager.NextState();
    }


    //private void OnCollisionEnter(Collision col)
    //{
    //    if (manager.currState == PlayerManagerScript.State.WAITING_FOR_THROW && (col.transform.CompareTag("Floor") || col.transform.CompareTag("Target")))
    //    {
    //        FinishState();
    //    }

    //}


    IEnumerator TimeOfGrace()
    {
        yield return new WaitForSeconds(5.0f);
        timeOfGrace = false;
        // ToDo: fer que tambe es reinici en X segons mes si no ha acabat encara 
    }

}

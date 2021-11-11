using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBallScript : MonoBehaviour
{
    public float maxRange = 5.0f;
    public float force = 10.0f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyUp(KeyCode.Mouse0) && Vector3.Distance(mousePos, transform.position) < maxRange)
        {
            Vector2 moveDir = (Input.mousePosition - transform.position).normalized;
            rb.isKinematic = false;
            rb.AddForce(moveDir * force, ForceMode.Impulse);
        }

    }
}

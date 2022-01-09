using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Vector3 speed = new Vector3(0.5f, 0.5f, 0.0f);
    public Vector3 rotSpeed = new Vector3(0.0f, 15.0f, 0.0f);
    public bool applyChangeRotDir = false;
    public Vector3 changeRotDirTimes = new Vector3(5.0f, 5.0f, 0.0f);
    public float frequency = 1f;
    public float stopOverTime = -1;

    private Vector3Int rotDir = new Vector3Int(1, 1, 1);

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    //Vector3 axisOffset = new Vector3();
    //float angleOffset;

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;

        //StartCoroutine(ChangeRotDirCoroutine());
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeRotDirCoroutine());

        if (stopOverTime > 0)
            StartCoroutine(StopOverTime());
    }


    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(Time.deltaTime * rotSpeed.x * rotDir.x, Time.deltaTime * rotSpeed.y * rotDir.y, Time.deltaTime * rotSpeed.z * rotDir.z), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * speed.x;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * speed.y;
        tempPos.z += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * speed.z;

        transform.position = tempPos;
    }


    IEnumerator ChangeRotDirCoroutine()
    {
        Vector3 rotTimers = new Vector3(0, 0, 0);

        while (applyChangeRotDir)
        {
            if (changeRotDirTimes.x < rotTimers.x)
            {
                rotTimers.x = 0;
                rotDir.x *= -1;
            }
            if (changeRotDirTimes.y < rotTimers.y)
            {
                rotTimers.y = 0;
                rotDir.y *= -1;
            }
            if (changeRotDirTimes.z < rotTimers.z)
            {
                rotTimers.z = 0;
                rotDir.z *= -1;
            }

            yield return new WaitForEndOfFrame();
            rotTimers.x += Time.deltaTime;
            rotTimers.y += Time.deltaTime;
            rotTimers.z += Time.deltaTime;
        }


    }

    IEnumerator StopOverTime()
    {
        float timeLeft = stopOverTime;
        Vector3 
            initSpeed = speed,
            initRotSpeed = rotSpeed;

        while (timeLeft > 0)
        {
            float lerpTime = (stopOverTime - timeLeft) / stopOverTime;
            speed = Vector3.Lerp(initSpeed, Vector3.zero, lerpTime);
            rotSpeed = Vector3.Lerp(initRotSpeed, Vector3.zero, lerpTime);

            yield return new WaitForEndOfFrame();
            timeLeft -= Time.deltaTime;

        }

        speed = Vector3.zero;
        rotSpeed = Vector3.zero;

    }

}

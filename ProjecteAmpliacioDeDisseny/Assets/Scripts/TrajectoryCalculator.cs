using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalculator : MonoBehaviour
{
    [SerializeField] float initExtraTime = 0.05f;
    [SerializeField] float timeDiff = 0.1f;
    [SerializeField] float scaleFactor = 1.8f;

    Transform[] trajectoryPoints;
    Vector3[] initScales;


    void Awake()
    {
        trajectoryPoints = GetComponentsInChildren<Transform>();

        initScales = new Vector3[trajectoryPoints.Length];
        for (int i = 0; i < initScales.Length; i++)
            initScales[i] = trajectoryPoints[i].localScale;
    }


    public void CalculateTrajectory(Vector2 _initPos, Vector2 _initForce, Vector2 _moveDir, float _mass)
    {
        for(int i = 0; i < trajectoryPoints.Length; i++)
        {
            float currTimeDiff = GetTimeDiff(i);

            // Position
            if (_moveDir != Vector2.zero)
            {
                //d = D(0) + V(0)*t + 1/2*a*t^2
                Vector2 initVel = ((_initForce * _moveDir) / _mass);
                Vector3 finalPos = new Vector2(
                    _initPos.x + (initVel.x * currTimeDiff),
                    _initPos.y + (initVel.y * currTimeDiff) + (0.5f * Physics.gravity.y * Mathf.Pow(currTimeDiff, 2))
                );
                trajectoryPoints[i].position = finalPos;

            }

            // Scale
            float scaleDecrease = scaleFactor * currTimeDiff;
            Vector3 newScale = initScales[i];
            newScale = new Vector3(newScale.x - scaleDecrease, newScale.y - scaleDecrease, newScale.z);
            if (newScale.x > 0.0f && newScale.y > 0.0f)
                trajectoryPoints[i].localScale = newScale;
            else
                trajectoryPoints[i].localScale = Vector3.zero;

        }

    }

    private float GetTimeDiff(int _it)
    {
        return initExtraTime + timeDiff * _it;
    }

}

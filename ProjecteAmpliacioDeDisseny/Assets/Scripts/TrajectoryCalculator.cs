using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalculator : MonoBehaviour
{
    [SerializeField] float initExtraTime = 0.05f;
    [SerializeField] float timeDiff = 0.1f;
    [SerializeField] float scaleFactor = 1.8f;
    [SerializeField] float rotFactor = -30.0f;
    [SerializeField] float alphaFactor = 0.2f;

    TrajectoryPoint[] trajectoryPoints;
    Vector3 initScale;
    Vector2 realScaleFactor;

    void Awake()
    {
        trajectoryPoints = GetComponentsInChildren<TrajectoryPoint>();
        realScaleFactor = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }


    public void CalculateTrajectory(Vector2 _initPos, Vector2 _initForce, float _mass)
    {
        for(int i = 0; i < trajectoryPoints.Length; i++)
        {
            float currTimeDiff = GetTimeDiff(i);

            // Position
            if (_initForce != Vector2.zero)
            {
                //d = D(0) + V(0)*t + 1/2*a*t^2
                Vector2 initVel = _initForce / _mass;
                Vector3 finalPos = new Vector2(
                    _initPos.x + (initVel.x * currTimeDiff),
                    _initPos.y + (initVel.y * currTimeDiff) + (0.5f * Physics.gravity.y * Mathf.Pow(currTimeDiff, 2))
                );
                trajectoryPoints[i].transform.position = finalPos;

            }

            // Scale
            Vector2 scaleDecrease = realScaleFactor * currTimeDiff;
            Vector3 newScale = initScale;
            newScale = new Vector3(newScale.x - scaleDecrease.x, newScale.y - scaleDecrease.y, newScale.z);
            if (newScale.x > 0.0f && newScale.y > 0.0f)
                trajectoryPoints[i].transform.localScale = newScale;
            else
                trajectoryPoints[i].transform.localScale = Vector3.zero;

        }

    }

    private float GetTimeDiff(int _it)
    {
        return initExtraTime + timeDiff * _it;
    }


    public void SetData(Mesh _mesh, Material _material, Vector3 _initScale)
    {
        initScale = _initScale;
        realScaleFactor = scaleFactor * initScale;
        Color newColor = _material.color;

        for(int i = 0; i < trajectoryPoints.Length; i++)
        {
            Mesh newMesh = _mesh;
            Material newMaterial = new Material(_material);
            newColor.a = 1 - (i + 1) * alphaFactor;
            newMaterial.color = newColor;

            trajectoryPoints[i].SetData(newMesh, newMaterial, (i + 1) * rotFactor);

            //trajectoryPoints[i].GetComponent<MeshFilter>().mesh = _mesh;
            //trajectoryPoints[i].GetComponent<MeshRenderer>().material = newMaterial;
        }

    }

}

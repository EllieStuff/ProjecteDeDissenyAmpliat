using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPoint : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }


    internal void SetData(Mesh _mesh, Material _material, float _rotAmount)
    {
        meshFilter.mesh = _mesh;
        meshRenderer.material = _material;
        transform.Rotate(0.0f, 0.0f, _rotAmount);
    }

}

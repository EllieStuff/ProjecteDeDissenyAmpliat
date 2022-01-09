using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValknutScript : MonoBehaviour
{
    internal int id;

    CollectiblesManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<CollectiblesManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            if (!manager.recorder.IsPlaying) manager.AddValknaut(id);
            gameObject.SetActive(false);
        }

    }


    internal void SetTextureAlpha(float _alpha)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material newMaterial = new Material(meshRenderer.material);
        newMaterial.color = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, _alpha);

        meshRenderer.material = newMaterial;
    }

}

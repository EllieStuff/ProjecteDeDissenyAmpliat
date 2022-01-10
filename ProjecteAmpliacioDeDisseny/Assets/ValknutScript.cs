using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValknutScript : MonoBehaviour
{
    internal int id;
    internal Color initColor;
    internal MeshRenderer meshRenderer;

    CollectiblesManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<CollectiblesManager>();
        meshRenderer = GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            if (!manager.recorder.IsPlaying) manager.AddValknaut(id);
            StartCoroutine(DestroyValknutCoroutine());
        }

    }


    internal void SetTextureAlpha(float _alpha)
    {
        Material newMaterial = new Material(meshRenderer.material);
        initColor = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, _alpha);
        newMaterial.color = initColor;

        meshRenderer.material = newMaterial;
    }


    IEnumerator DestroyValknutCoroutine()
    {
        float destroyDelay = 0.05f;
        float timer = 0.0f;
        Color newColor = initColor;

        while (timer < destroyDelay)
        {
            newColor.a = Mathf.Lerp(initColor.a, 0.1f, timer / destroyDelay);
            meshRenderer.material.color = newColor;

            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithObjective : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            AudioManager.Play_SFX("SoldierDeathShort_SFX", true, Random.Range(0.85f, 1.3f));
            GameObject.Find("Canvas").GetComponent<EndLevel>().HasCollided();
            GameObject.FindGameObjectWithTag("CollectiblesManager").GetComponent<CollectiblesManager>().SaveValknauts();
        }
    }

}

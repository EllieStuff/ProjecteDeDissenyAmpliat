using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithObjective : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            GameObject.Find("Canvas").GetComponent<EndLevel>().HasCollided();
            GameObject.FindGameObjectWithTag("CollectiblesManager").GetComponent<CollectiblesManager>().SaveValknauts();
        }
    }
}

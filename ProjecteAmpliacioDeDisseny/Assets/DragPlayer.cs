using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlayer : MonoBehaviour
{
    [SerializeField] Transform initPlayerLocation;

    Vector3 potentialPlayerLocation;
    Vector3 currPlayerLocation;
    bool touchingPlayerLocation = false;


    // Start is called before the first frame update
    void Awake()
    {
        transform.position = currPlayerLocation = initPlayerLocation.GetChild(0).position;
    }


    private void OnMouseDown()
    {
        AudioManager.Play_SFX("PickUpItem_SFX");
    }

    private void OnMouseDrag()
    {
        if (this.enabled)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }

    }

    private void OnMouseUp()
    {
        if (touchingPlayerLocation)
        {
            touchingPlayerLocation = false;
            transform.position = currPlayerLocation = potentialPlayerLocation;
        }
        else
        {
            transform.position = currPlayerLocation;
        }

        AudioManager.Play_SFX("LetDownItem_SFX");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerLocations")
        {
            touchingPlayerLocation = true;
            potentialPlayerLocation = other.transform.GetChild(0).position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerLocations")
        {
            touchingPlayerLocation = false;
        }
    }

}

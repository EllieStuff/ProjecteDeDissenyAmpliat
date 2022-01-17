using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlayer : MonoBehaviour
{
    [SerializeField] Transform initPlayerLocation;

    PlayerManagerScript manager;
    Vector3 potentialPlayerLocation;
    Vector3 currPlayerLocation;
    bool touchingPlayerLocation = false;


    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<PlayerManagerScript>();
        transform.position = currPlayerLocation = initPlayerLocation.GetChild(0).position;
    }


    private void OnMouseDown()
    {
        AudioManager.PlayRandomEinarGenericLine();
    }

    private void OnMouseDrag()
    {
        if (this.enabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1 << 7))
            {
                transform.position = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z);
            }
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

        if (manager.currState == PlayerManagerScript.State.EDITING_POS)
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

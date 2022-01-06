using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour
{
    internal int id;

    ChooseWeaponScript manager;
    bool touchingPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<ChooseWeaponScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

    }

    private void OnMouseUp()
    {
        if (touchingPlayer)
        {
            touchingPlayer = false;
            manager.ChangeDisabledItem(id);
        }
        else
        {
            manager.RefreshWeaponsList();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            touchingPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            touchingPlayer = false;
        }
    }


}

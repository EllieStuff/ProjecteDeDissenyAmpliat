using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour
{
    internal int id;
    internal bool touchingPlayer = false;

    ChooseWeaponScript manager;


    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<ChooseWeaponScript>();
    }


    private void OnMouseDown()
    {
        AudioManager.Play_SFX("PickUpItem_SFX");
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mousePos);
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

        AudioManager.Play_SFX("LetDownItem_SFX");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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

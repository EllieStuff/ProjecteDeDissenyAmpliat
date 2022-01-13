using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWeaponScript : MonoBehaviour
{
    //internal bool refreshFlag = true;

    PlayerManagerScript playerManager;
    Transform[] weaponsList;
    Vector3[] weaponsPos;
    int currDisabledIdx = -1;
    public int CurrUsedIdx { get { return currDisabledIdx; } }
    public Transform[] WeaponList { get { return weaponsList; } }
    public int WeaponsAmount { get { return weaponsList.Length; } }
    public Vector3[] WeaponsInitPos { get { return weaponsPos; } }


    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManagerScript>();
        weaponsList = new Transform[transform.childCount];
        weaponsPos = new Vector3[weaponsList.Length];
        for(int i = 0; i < weaponsList.Length; i++)
        {
            weaponsList[i] = transform.GetChild(i);
            weaponsPos[i] = weaponsList[i].position;

            weaponsList[i].GetComponent<DragItem>().id = i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (refreshFlag)
        //{
        //    refreshFlag = false;

        //    RefreshWeaponsList();

        //}

    }


    public void RefreshWeaponsList()
    {
        int posIdx = 0;
        for (int i = 0; i < weaponsList.Length; i++)
        {
            if (weaponsList[i].gameObject.activeSelf)
            {
                weaponsList[i].position = weaponsPos[posIdx];
                posIdx++;
            }

        }

    }


    public void ChangeDisabledItem(int _idx)
    {
        if (currDisabledIdx >= 0)
            weaponsList[currDisabledIdx].gameObject.SetActive(true);
        currDisabledIdx = _idx;
        weaponsList[currDisabledIdx].gameObject.SetActive(false);

        RefreshWeaponsList();
        // ToDo: Change Players Weapon

    }

    public Vector2[] GetWeaponsCurrentPos()
    {
        Vector2[] posList = new Vector2[weaponsList.Length];
        for (int i = 0; i < posList.Length; i++)
            posList[i] = weaponsList[i].position;

        return posList;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public bool isStored = false;

    public enum ItemTypeList
    {
        None,
        Weapon,
        Barrel,
        CanonBall,
        Wood,
        Canon,
        Crate,
        Plank
    }

    public ItemTypeList itemType;

    private void Awake()
    {
        if(itemType != ItemTypeList.Plank)
            isStored = true;
    }

}

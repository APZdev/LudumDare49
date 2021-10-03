using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public bool isStored;

    public enum ItemTypeList
    {
        Weapon,
        Barrel,
        CanonBall,
        Wood,
        Canon,
    }

    public ItemTypeList itemType;

    private void Awake() => isStored = true;
}

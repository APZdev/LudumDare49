using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGroupManager : MonoBehaviour
{
    private GameEssentials gameEssentials;

    public List<GameObject> physicsItems = new List<GameObject>();
    public bool collisionObject = true;


    private Vector3[] slotLocalPositions;
    private Vector3[] slotLocalRotation;

    public ItemType.ItemTypeList itemGroupType;

    private void Start()
    {
        gameEssentials = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameEssentials>();

        slotLocalPositions = new Vector3[physicsItems.Count];
        slotLocalRotation = new Vector3[physicsItems.Count];

        //Register item slot local positioin
        for (int i = 0; i < physicsItems.Count; i++)
        {
            if(physicsItems[i] != null)
            {
                slotLocalPositions[i] = physicsItems[i].transform.localPosition;
                slotLocalRotation[i] = physicsItems[i].transform.localEulerAngles;

                if (!collisionObject)
                {
                    //SortingLayer.NameToID("IgnorePlayerCollision") just doesn't work....
                    physicsItems[i].layer = 7; //Ignore collision with player
                }
            }
        }

        FreeItem(5); //Test
    }

    public int SetObjectKinematicState(GameObject go, bool state)
    {
        go.GetComponent<Rigidbody>().isKinematic = state;
        return 1;
    }

    public void StoreItem(GameObject go)
    {
        for (int i = 0; i < physicsItems.Count; i++)
        {
            if(physicsItems[i] == null && go.GetComponent<ItemType>()) //Check if object exists on slot and if it has a itemType component
            {
                if(itemGroupType == go.GetComponent<ItemType>().itemType) //Check if object is the same type as the group
                {
                    physicsItems[i] = go;
                    go.transform.parent = transform;
                    go.transform.localPosition = slotLocalPositions[i];
                    go.transform.localEulerAngles = slotLocalRotation[i];
                    physicsItems[i].GetComponent<ItemType>().isStored = true;

                    gameEssentials.playerInteractions.isHoldingItem = false;
                    gameEssentials.playerInteractions.holdingObject = null;

                    return;
                }
            }
        }
    }

    public void FreeItem(int amount)
    {
        int count = amount;

        for (int i = 0; i < physicsItems.Count; i++)
        {
            if (count < 1) return; //Don't change object state if we already changed the correct amount

            if (physicsItems[i] != null)
            {
                count -= SetObjectKinematicState(physicsItems[i], false);
                physicsItems[i].GetComponent<ItemType>().isStored = false;
                physicsItems[i] = null;
            }
        }
    }
}

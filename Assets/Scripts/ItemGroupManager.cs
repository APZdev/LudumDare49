using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGroupManager : MonoBehaviour
{
    private GameEssentials gameEssentials;
    private GameManager gameManager;

    public List<GameObject> physicsItems = new List<GameObject>();
    public bool collisionObject = true;


    private Vector3[] slotLocalPositions;
    private Vector3[] slotLocalRotation;
    public ItemType.ItemTypeList itemGroupType;


    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameEssentials = gameManager.GetComponent<GameEssentials>();

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

        FreeItem(6); //Test
    }

    public int SetObjectKinematicState(GameObject go, bool state)
    {
        go.GetComponent<Rigidbody>().isKinematic = state;
        return 1;
    }

    public void StoreItem(GameObject go)
    {
        for (int i = physicsItems.Count - 1; i >= 0; i--)
        {
            if(physicsItems[i] == null && go.GetComponent<ItemType>()) //Check if object exists on slot and if it has a itemType component
            {
                if(itemGroupType == go.GetComponent<ItemType>().itemType) //Check if object is the same type as the group
                {
                    gameEssentials.interactionAudioSource.PlayOneShot(gameEssentials.storeSound, 0.6f);
                    physicsItems[i] = go;
                    go.transform.parent = transform;
                    go.transform.localPosition = slotLocalPositions[i];
                    go.transform.localEulerAngles = slotLocalRotation[i];
                    physicsItems[i].GetComponent<ItemType>().isStored = true;

                    gameEssentials.playerInteractions.isHoldingItem = false;
                    gameEssentials.playerInteractions.holdingObject = null;

                    gameManager.AddScore(5);

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

            if(physicsItems[i] != null)
            {
                if (physicsItems[i].GetComponent<ItemType>())
                {
                    physicsItems[i].GetComponent<ItemType>().isStored = false;
                    count -= SetObjectKinematicState(physicsItems[i], false);
                    physicsItems[i].GetComponent<Collider>().isTrigger = false;
                    physicsItems[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
                    physicsItems[i].GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(3.5f, 3.5f, 3.5f), ForceMode.Impulse);
                    physicsItems[i] = null;
                }
            }
        }
    }
}

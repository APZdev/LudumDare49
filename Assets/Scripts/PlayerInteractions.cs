using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    private GameEssentials gameEssentials;
    private AudioSource audioSource;

    public Transform handTarget;
    public Transform shipTarget;
    public AudioClip pickupSound;
    public AudioClip repairShipSound;

    public GameObject plankPrefab;

    public bool isHoldingItem;
    public GameObject holdingObject;
    public ItemType.ItemTypeList holdingItemType;


    private void Start()
    {
        isHoldingItem = false;

        audioSource = GetComponent<AudioSource>();
        gameEssentials = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameEssentials>();
    }

    private void Update()
    {
        ElectricSwitch();
        ElevatorState();
        RepairShip();
        DetectNearbyItem();
        WoodStorage();
    }

    private void ElevatorState()
    {
        if (gameEssentials.elevatorController.canUseElevator)
        {
            if(Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if(hit.collider.gameObject.tag == "ElevatorUp")
                        gameEssentials.elevatorController.OnCall_ElevatorUp();
                    else if (hit.collider.gameObject.tag == "ElevatorDown")
                        gameEssentials.elevatorController.OnCall_ElevatorDown();
                }
            }
        }
    }

    private void ElectricSwitch()
    {
        if(gameEssentials.electricSwitch.canChange)
        {
            if(Input.GetMouseButtonDown(0))
                gameEssentials.electricSwitch.switchOn = !gameEssentials.electricSwitch.switchOn;
        }
    }

    private void WoodStorage()
    {
        isHoldingItem = holdingObject != null ? true : false;

        if (gameEssentials.woodStorageManager.canPickupWood && !isHoldingItem)
        {
            if (Input.GetMouseButtonDown(0) && gameEssentials.woodStorageManager.woodRessources > 0)
            {
                GameObject plankObject = Instantiate(plankPrefab, handTarget);
                gameEssentials.woodStorageManager.woodRessources--;

                audioSource.PlayOneShot(pickupSound, 1f);

                plankObject.GetComponent<Rigidbody>().isKinematic = true;
                plankObject.GetComponent<Collider>().isTrigger = true;
                plankObject.transform.localPosition = Vector3.zero;

                holdingItemType = plankObject.GetComponent<ItemType>().itemType;
                holdingObject = plankObject.gameObject;
            }
        }
    }

    private void RepairShip()
    {
        if(isHoldingItem && holdingItemType == ItemType.ItemTypeList.Plank)
        {
            Collider[] hitObjects = Physics.OverlapBox(transform.position, new Vector3(2f, 2f, 2f), Quaternion.identity);

            foreach (Collider hit in hitObjects)
            {
                if(hit.GetComponent<WaterHoleInfo>())
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        //Destroy waterHole
                        Destroy(hit.gameObject);

                        //Reset the waterHole state to make it available for another ship hit
                        gameEssentials.gameManager.shipDamagePointsStates[hit.GetComponent<WaterHoleInfo>().waterHoleId] = false;

                        audioSource.PlayOneShot(repairShipSound, 1f);

                        //Add score
                        gameEssentials.gameManager.AddScore(10);

                        //Destroy plank in hand
                        Destroy(holdingObject);
                        holdingObject = null;

                    }
                }
            }
        }
    }

    private void DetectNearbyItem()
    {
        isHoldingItem = holdingObject != null ? true : false;

        Collider[] hitObjects = Physics.OverlapBox(transform.position, new Vector3(2f, 2f, 2f), Quaternion.identity);

        if (Input.GetMouseButtonDown(0)) //Check if button got pressed for 1 sec
        {
            foreach (Collider hit in hitObjects)
            {
                //Check only non stored items
                if (!isHoldingItem && hit.GetComponent<ItemType>() && !hit.GetComponent<ItemType>().isStored)
                {
                    audioSource.PlayOneShot(pickupSound, 1f);

                    hit.GetComponent<Rigidbody>().isKinematic = true;
                    hit.isTrigger = true;
                    hit.transform.SetParent(handTarget);
                    hit.transform.localPosition = Vector3.zero;

                    holdingItemType = hit.GetComponent<ItemType>().itemType;
                    holdingObject = hit.gameObject;
                    return;
                }
                else if (isHoldingItem && holdingObject != null)
                {
                    if(hit.GetComponent<ItemGroupManager>())
                    {
                        hit.GetComponent<ItemGroupManager>().StoreItem(holdingObject);
                    }
                    //Check that we are not in the elevator nor in front of the electrical switch
                    else if (!gameEssentials.elevatorController.canUseElevator && !gameEssentials.electricSwitch.canChange) //Drop item
                    { 
                        if(holdingItemType == ItemType.ItemTypeList.Plank)
                        {
                            holdingObject.transform.SetParent(shipTarget);
                            holdingObject.GetComponent<Rigidbody>().isKinematic = false;
                            holdingObject.GetComponent<Collider>().isTrigger = false;
                            holdingObject.GetComponent<ItemType>().isStored = false;
                            Destroy(holdingObject, 2f);

                        }
                        else
                        {
                            holdingObject.transform.SetParent(shipTarget);
                            holdingObject.GetComponent<Rigidbody>().isKinematic = false;
                            holdingObject.GetComponent<Collider>().isTrigger = false;
                            holdingObject = null;
                        }

                    }
                    return;
                }
            }
        }
    }
}

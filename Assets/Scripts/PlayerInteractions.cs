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


    [HideInInspector] public bool isHoldingItem;
    [HideInInspector] public GameObject holdingObject;
    private ItemType.ItemTypeList holdingItemType;


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
        DetectNearbyItem();
    }

    private void ElevatorState()
    {
        if (gameEssentials.elevatorController.canUseElevator)
        {
            if(Input.GetMouseButtonUp(0))
            {
                if (Input.mousePosition.y >= Screen.currentResolution.height / 2)
                    gameEssentials.elevatorController.OnCall_ElevatorUp();
                else
                    gameEssentials.elevatorController.OnCall_ElevatorDown();
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

    private void DetectNearbyItem()
    {
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

                    isHoldingItem = true;
                    holdingItemType = hit.GetComponent<ItemType>().itemType;
                    holdingObject = hit.gameObject;
                    return;
                }
                else if (isHoldingItem)
                {
                    if(hit.GetComponent<ItemGroupManager>())
                    {
                        hit.GetComponent<ItemGroupManager>().StoreItem(holdingObject);
                    }
                    //Check that we are not in the elevator nor in front of the electrical switch
                    else if (!gameEssentials.elevatorController.canUseElevator && !gameEssentials.electricSwitch.canChange)
                    { 
                        holdingObject.transform.SetParent(shipTarget);
                        holdingObject.GetComponent<Rigidbody>().isKinematic = false;
                        holdingObject.GetComponent<Collider>().isTrigger = false;
                        isHoldingItem = false;
                        holdingObject = null;
                    }
                    return;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    private GameEssentials gameEssentials;

    public Image leftCooldownBar;
    public Image rightCooldownBar;

    private float elapsedCooldownTime;

    public Transform handTarget;
    [HideInInspector] public bool isHoldingItem;
    [HideInInspector] public GameObject holdingObject;
    private ItemType.ItemTypeList holdingItemType;


    private void Start()
    {
        gameEssentials = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameEssentials>();
    }

    private void Update()
    {
        ElevatorState();
        ElectricSwitch();
        DetectNearbyItem();
    }

    private void ElevatorState()
    {
        if (gameEssentials.elevatorController.canUseElevator)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                gameEssentials.elevatorController.OnCall_ElevatorUp();
            else if (Input.GetKeyDown(KeyCode.S))
                gameEssentials.elevatorController.OnCall_ElevatorDown();
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

        SetCooldownBar(Color.red, 1f);
        foreach(Collider hit in hitObjects)
        {
            if (hit.GetComponent<ItemType>() && !isHoldingItem)
            {
                if(!hit.GetComponent<ItemType>().isStored) //Check only non stored items
                {
                    elapsedCooldownTime = Input.GetMouseButton(0) ? elapsedCooldownTime + Time.deltaTime : 0;

                    SetCooldownBar(Color.green, 1 - elapsedCooldownTime);

                    if(elapsedCooldownTime > 1) //Check if button got pressed for 1 sec
                    {
                        elapsedCooldownTime = 0;
                        hit.GetComponent<Rigidbody>().isKinematic = true;
                        hit.isTrigger = true;
                        hit.transform.SetParent(handTarget);
                        hit.transform.localPosition = Vector3.zero;

                        isHoldingItem = true;
                        holdingItemType = hit.GetComponent<ItemType>().itemType;
                        holdingObject = hit.gameObject;
                    }
                }
            }
            else if(hit.GetComponent<ItemGroupManager>() && isHoldingItem)
            {
                if(Input.GetMouseButtonDown(0))
                    hit.GetComponent<ItemGroupManager>().StoreItem(holdingObject);
            }
        }
    }

    private void SetCooldownBar(Color barColor, float fillValue)
    {
        leftCooldownBar.color = barColor;
        rightCooldownBar.color = barColor;

        leftCooldownBar.fillAmount = fillValue;
        rightCooldownBar.fillAmount = fillValue;
    }
}

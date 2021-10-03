using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private GameEssentials gameEssentials;

    public float elevatorSpeed = 5;
    public float[] wayPoints = new float[3];
    public Transform elevatorObject;
    public Transform playerContainerObject;

    [HideInInspector] public bool canUseElevator;

    private int currentLevel;
    private Vector3 targetPosition;

    private int tempLevel;

    private void Start()
    {
        gameEssentials = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameEssentials>();
        targetPosition = new Vector3(elevatorObject.localPosition.x, wayPoints[currentLevel], elevatorObject.localPosition.z);
        canUseElevator = false;
    }

    private void Update()
    {
        if(!gameEssentials.electricSwitch.switchOn)
            currentLevel = 0;

        elevatorObject.localPosition = Vector3.Lerp(elevatorObject.localPosition, new Vector3(elevatorObject.localPosition.x, targetPosition.y, elevatorObject.localPosition.z), elevatorSpeed * Time.deltaTime);

        currentLevel = Mathf.Clamp(currentLevel, 0, 2);

        if(tempLevel != currentLevel) //Check if elevator level changed
            targetPosition = new Vector3(elevatorObject.localPosition.x, wayPoints[currentLevel], elevatorObject.localPosition.z);
        tempLevel = currentLevel;

        //Set player parent of the elevator so he doesn't clip through it
        if(canUseElevator)
            gameEssentials.playerObject.transform.SetParent(elevatorObject);
        else
            gameEssentials.playerObject.transform.SetParent(playerContainerObject);
    }

    public void OnCall_ElevatorUp() => currentLevel++;

    public void OnCall_ElevatorDown() => currentLevel--;

    //Can use elevator only if we are on the elevator trigger and if electricity is turned on
    public void SetCanUseElevator(bool state) => canUseElevator = state && gameEssentials.electricSwitch.switchOn;
}

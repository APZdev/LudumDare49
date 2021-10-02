using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float elevatorSpeed = 5;
    public GameObject[] wayPoints = new GameObject[3];

    private Vector3 targetPosition;



    private void Start()
    {
        targetPosition = wayPoints[0].transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, elevatorSpeed * Time.deltaTime);
    }

    public void OnCall_ElevatorUp()
    {

    }

    private void SetElevatorLevel(int levelIndex)
    {
        if (levelIndex > wayPoints.Length) 
            levelIndex = wayPoints.Length;

        if (levelIndex < 0)
            levelIndex = 0;

        targetPosition = wayPoints[0].transform.position;
    }
}

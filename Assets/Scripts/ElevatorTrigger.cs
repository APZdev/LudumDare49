using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private ElevatorController elevatorController;

    private void Start() => elevatorController = GetComponentInParent<ElevatorController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            elevatorController.SetCanUseElevator(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            elevatorController.SetCanUseElevator(false);
    }
}

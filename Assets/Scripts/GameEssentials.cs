using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEssentials : MonoBehaviour
{
    public ElevatorController elevatorController;
    public ElectricSwitch electricSwitch;
    public PlayerInteractions playerInteractions;
    public GameObject playerObject;

    public AudioSource interactionAudioSource;
    public AudioClip storeSound;
}

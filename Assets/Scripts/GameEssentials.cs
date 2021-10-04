using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEssentials : MonoBehaviour
{
    public GameManager gameManager;
    public GameUIManager gameUIManager;
    public ElevatorController elevatorController;
    public ElectricSwitch electricSwitch;
    public PlayerInteractions playerInteractions;
    public CameraEffects cameraEffects;
    public WoodStorageManager woodStorageManager;
    public GameObject playerObject;

    public AudioSource interactionAudioSource;
    public AudioClip storeSound;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WoodStorageManager : MonoBehaviour
{
    public int woodRessources = 0;
    public TextMeshPro woodIndicator;

    public bool canPickupWood;

    private float woodCooldownElapsedTime;


    private void Start()
    {
        woodRessources = 0;
        canPickupWood = false;
    }

    private void Update()
    {
        woodRessources = Mathf.Clamp(woodRessources, 0, 1000);
        WoodRessourceManager();

        woodIndicator.text = woodRessources.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            canPickupWood = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            canPickupWood = false;
    }

    private void WoodRessourceManager()
    {
        woodCooldownElapsedTime += Time.deltaTime;

        if (woodCooldownElapsedTime > 10)
        {
            woodRessources++;
            woodCooldownElapsedTime = 0;
        }
    }
}

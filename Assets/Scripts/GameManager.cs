using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int woodRessources = 0;
    public int playerStamina = 100;
    public int totalScore = 0;
    public int boatStability = 100;

    public TextMeshPro woodIndicator;
    public Image staminaBar;
    public TextMeshProUGUI scoreTxt;
    public Image boatStabilityBar;

    public GameObject[] registeredItems;

    private float woodCooldownElapsedTime;

    private void Start()
    {
        registeredItems = GameObject.FindGameObjectsWithTag("PickableItem");
    }

    private void Update()
    {
        int storedItemsCount = 0;
        for (int i = 0; i < registeredItems.Length; i++)
        {
            if (registeredItems[i].GetComponent<ItemType>().isStored)
                storedItemsCount++;
        }
        Debug.Log($"{storedItemsCount} : {registeredItems.Length}");

        UpdateUI();
        WoodRessourceManager();
    }

    private void UpdateUI()
    {
        woodIndicator.text = woodRessources.ToString();
        staminaBar.fillAmount = playerStamina / 100f;
        scoreTxt.text = totalScore.ToString();
        boatStabilityBar.fillAmount = boatStability / 100f;
    }

    private void WoodRessourceManager()
    {
        woodCooldownElapsedTime += Time.deltaTime;

        if(woodCooldownElapsedTime > 5)
        {
            woodRessources++;
            woodCooldownElapsedTime = 0;
        }
    }

    public void AddScore(int amount) => totalScore += amount;
}

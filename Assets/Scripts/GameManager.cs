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

    public TextMeshProUGUI woodRessourcesTxt;
    public Image staminaBar;
    public TextMeshProUGUI scoreTxt;
    public Image boatStabilityBar;

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        woodRessourcesTxt.text = woodRessources.ToString();
        staminaBar.fillAmount = playerStamina / 100f;
        scoreTxt.text = totalScore.ToString();
        boatStabilityBar.fillAmount = boatStability / 100f;
    }
}

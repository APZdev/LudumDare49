using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameEssentials gameEssentials;

    public AudioSource audioSource;
    public AudioClip canonSoundClip;
    public AudioClip[] shipHitClips;

    public int maxHoles;
    public int currentHolesNumber;
    public int totalScore = 0;
    public float boatStability = 1;

    public TextMeshProUGUI scoreTxt;
    public Image shipStabilityBar;
    public TextMeshProUGUI shipStabilityText;

    public int attackStartTimeout;
    public int attackStartCooldown;
    public int attackCounter = 1;


    private GameObject[] registeredItems;
    private ItemGroupManager[] registeredItemGroups;


    public Transform[] shipDamagePoints;
    [HideInInspector] public bool[] shipDamagePointsStates;
    public GameObject waterHolePrefab;


    private void Start()
    {
        gameEssentials = GetComponent<GameEssentials>();
        registeredItems = GameObject.FindGameObjectsWithTag("PickableItem");
        registeredItemGroups = FindObjectsOfType<ItemGroupManager>();

        currentHolesNumber = 0;


        shipDamagePointsStates = new bool[shipDamagePoints.Length];
        StartCoroutine(ShipAttack());
    }

    private void Update()
    {
        GetShipStability();
        UpdateUI();

        if (boatStability < 0.10f)
        {
            Debug.Log("You Lose");
        }
    }

    private void UpdateUI()
    {
        scoreTxt.text = totalScore.ToString();

        shipStabilityBar.fillAmount = boatStability;
    }

    private void GetShipStability()
    {
        int storedItemsCount = 0;
        for (int i = 0; i < registeredItems.Length; i++)
        {
            if (registeredItems[i].GetComponent<ItemType>().isStored)
                storedItemsCount++;
        }

        boatStability = (float)(storedItemsCount + maxHoles - currentHolesNumber) / (registeredItems.Length + maxHoles);  
        shipStabilityText.text = $"{storedItemsCount + maxHoles}/{registeredItems.Length + maxHoles}";
    }

    private IEnumerator ShipAttack()
    {
        attackCounter = 1;
        yield return new WaitForSeconds(attackStartTimeout);

        while (true)
        {
            float currentAttackRate = attackCounter > attackStartCooldown ? 1f : attackStartCooldown - (1 / 1.25f) * attackCounter;

            audioSource.PlayOneShot(canonSoundClip, 0.4f);
            yield return new WaitForSeconds(0.75f);
            audioSource.PlayOneShot(shipHitClips[Random.Range(0, shipHitClips.Length)], 0.2f);
            yield return new WaitForSeconds(0.25f);
            gameEssentials.cameraEffects.Shake();
            currentHolesNumber++;
            currentHolesNumber = Mathf.Clamp(currentHolesNumber, 0, maxHoles);

            //Free some objects randomly
            int[] targetedGroups = { 0, -1 };
            targetedGroups[0] = Random.Range(0, registeredItemGroups.Length);
            do
            {
                targetedGroups[1] = Random.Range(0, registeredItemGroups.Length);
            }
            while (targetedGroups[0] != targetedGroups[1]);

            foreach(int groupId in targetedGroups)
            {
                registeredItemGroups[groupId].FreeItem(Random.Range(1, 3));
            }

            //Create water hole in the ship randomly
            int randomPoint = 0;
            do
            {
                randomPoint = Random.Range(0, shipDamagePoints.Length);
            } 
            while (shipDamagePointsStates[randomPoint] && currentHolesNumber < maxHoles);

            //1 chance out of 3 to turn off the electricity
            int elevatorSwitch = Random.Range(0, 3);
            if(elevatorSwitch == 1)
            {
                gameEssentials.electricSwitch.switchOn = false;
            }

            GameObject waterHole = Instantiate(waterHolePrefab, shipDamagePoints[randomPoint]);
            waterHole.GetComponent<WaterHoleInfo>().waterHoleId = randomPoint;
            waterHole.transform.localPosition = Vector3.zero;
            waterHole.transform.localEulerAngles = new Vector3(0, 90, 0);
            shipDamagePointsStates[randomPoint] = true;

            yield return new WaitForSeconds(currentAttackRate);

            attackCounter++;
        }
    }

    public void AddScore(int amount) => totalScore += amount;
}

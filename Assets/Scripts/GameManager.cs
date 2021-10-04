using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameEssentials gameEssentials;

    public Animator endGameAnim;
    public Animator endGameUIAnim;

    public AudioSource audioSource;
    public AudioClip canonSoundClip;
    public AudioClip[] shipHitClips;

    public int maxHoles;
    public int currentHolesNumber;
    public int totalScore = 0;
    public float boatStability = 1;

    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI endGameScoreTxt;
    public Image shipStabilityBar;

    public int attackStartTimeout;
    public int attackStartCooldown;
    public int attackCounter = 1;

    public GameObject indicatorLight;


    private GameObject[] registeredItems;
    private ItemGroupManager[] registeredItemGroups;


    public Transform[] shipDamagePoints;
    [HideInInspector] public bool[] shipDamagePointsStates;
    public GameObject waterHolePrefab;

    public bool isPlaying;
    public bool gameIsPaused;
    public GameObject pauseMenu;

    private void Start()
    {
        gameEssentials = GetComponent<GameEssentials>();
        registeredItems = GameObject.FindGameObjectsWithTag("PickableItem");
        registeredItemGroups = FindObjectsOfType<ItemGroupManager>();

        currentHolesNumber = 0;
        isPlaying = true;

        shipDamagePointsStates = new bool[shipDamagePoints.Length];
        StartCoroutine(ShipAttack());
    }

    private void Update()
    {
        GetShipStability();
        ShipMovingWithStability();
        ShipSinkCheck();
        PlaceIndicatorLight();

        ChangePauseMenuState();
        UpdateUI();
    }

    private void ChangePauseMenuState()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isPlaying)
        {
            gameIsPaused = !gameIsPaused;
        }

        pauseMenu.SetActive(gameIsPaused);
        int state = gameIsPaused ? 0 : 1;
        Time.timeScale = state;
    }

    public void SetPauseGameState(bool state) => gameIsPaused = state;

    private void ShipSinkCheck()
    {
        if (boatStability < 0.1f)
        {
            endGameAnim.CrossFade("WaterCover", 0);
            endGameUIAnim.CrossFade("WaterCover", 0);

            isPlaying = false;
            Time.timeScale = 0;
        }
    }

    private void UpdateUI()
    {
        scoreTxt.text = totalScore.ToString();
        endGameScoreTxt.text = $"SCORE : {totalScore}";

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
    }

    private void ShipMovingWithStability()
    {
        gameEssentials.shipController.waveAmount = 6 * (1 - boatStability);
        gameEssentials.shipController.rotAngle = 35 * (1 - boatStability);
        gameEssentials.shipController.rotSpeed = 2 * (1 - boatStability);
        gameEssentials.shipController.rotSmooth = 5 * (1 - boatStability);
    }

    private IEnumerator ShipAttack()
    {
        attackCounter = 1;
        yield return new WaitForSeconds(attackStartTimeout);

        while (isPlaying)
        {
            audioSource.PlayOneShot(canonSoundClip, 0.2f);
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
            while (targetedGroups[0] == targetedGroups[1]);

            foreach(int groupId in targetedGroups)
            {
                registeredItemGroups[groupId].FreeItem(Random.Range(1, 3));
            }

            if(currentHolesNumber < maxHoles)
            {
                //Create water hole in the ship randomly
                int randomPoint = 0;
                do
                {
                    randomPoint = Random.Range(0, shipDamagePoints.Length);
                } 
                while (shipDamagePointsStates[randomPoint]);

                GameObject waterHole = Instantiate(waterHolePrefab, shipDamagePoints[randomPoint]);
                waterHole.GetComponent<WaterHoleInfo>().waterHoleId = randomPoint;
                waterHole.transform.localPosition = Vector3.zero;
                waterHole.transform.localEulerAngles = new Vector3(0, 90, 0);
                shipDamagePointsStates[randomPoint] = true;
            }

            //1 chance out of 3 to turn off the electricity
            int elevatorSwitch = Random.Range(0, 3);
            if (elevatorSwitch == 1)
            {
                gameEssentials.electricSwitch.switchOn = false;
            }

            float currentAttackRate = attackStartCooldown - (1 / 105 * Mathf.Pow(attackCounter, 2));

            if (currentAttackRate <= 0)
                currentAttackRate = 3f;

            yield return new WaitForSeconds(currentAttackRate);

            attackCounter++;
        }
    }

    private void PlaceIndicatorLight()
    {
        for (int i = 0; i < registeredItemGroups.Length; i++)
        {
            if(registeredItemGroups[i].GetComponent<ItemGroupManager>().itemGroupType == gameEssentials.playerInteractions.holdingItemType)
            {
                indicatorLight.SetActive(true);
                indicatorLight.transform.position = new Vector3(registeredItemGroups[i].transform.position.x, 
                                                                registeredItemGroups[i].transform.position.y, 
                                                                indicatorLight.transform.position.z) +
                                                                Vector3.up * 3f;
            }
            else if(gameEssentials.playerInteractions.holdingItemType == ItemType.ItemTypeList.None)
            {
                indicatorLight.SetActive(false);
            }
        }
    }

    public void AddScore(int amount) => totalScore += amount;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI resourceText;
    
    public TMPro.TextMeshProUGUI spawnCollectorButtonText;
    //initialize the button
    public GameObject spawnCollectorButton;
    // Start is called before the first frame update

    public GameObject upgradeCollectorSpeedButton;
    public GameObject upgradeCollectorCapacityButton;
    public GameObject upgradeCollectorLoadRateButton;
    public GameObject upgradeCollectorOffloadRateButton;

    public GameManager gameManager;

    void Start()
    {
        resourceText = GameObject.Find("UIResourceText").GetComponent<TextMeshProUGUI>();
        spawnCollectorButton = GameObject.Find("SpawnCollectorButton");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnCollectorButtonText = spawnCollectorButton.GetComponentInChildren<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        updateResourceText(gameManager.resourceAmount);
        updateCollectorButton(gameManager.resourceAmount, gameManager.calculateCollectorCost());
        updateCollectorSpeedButtonText(gameManager.collectorMovementSpeed,
            gameManager.calculateUpgradeCost(gameManager.collectorMovementSpeedLevel));
        updateCollectorCapacityButtonText(gameManager.collectorResourceCapacity,
            gameManager.calculateUpgradeCost(gameManager.collectorResourceCapacityLevel));
        updateCollectorLoadRateButtonText(gameManager.collectorResourceGatherRate,
            gameManager.calculateUpgradeCost(gameManager.collectorResourceGatherRateLevel));
        updateCollectorOffloadRateButtonText(gameManager.collectorResourceOffloadRate,
            gameManager.calculateUpgradeCost(gameManager.collectorResourceOffloadRateLevel));
    }

    public void updateResourceText(float resourceAmount)
    {
        resourceText.text = "Resources: " + resourceAmount.ToString();
    }

    public void updateCollectorSpeedButtonText(float collectorMovementSpeed, float upgradeCost)
    {
        upgradeCollectorSpeedButton.GetComponentInChildren<TextMeshProUGUI>().text = 
        collectorMovementSpeed.ToString() + " (" + upgradeCost.ToString() + ")";
    }
    
    public void updateCollectorCapacityButtonText(float collectorResourceCapacity,   float upgradeCost)    
    {
        upgradeCollectorCapacityButton.GetComponentInChildren<TextMeshProUGUI>().text =  collectorResourceCapacity.ToString()+ " (" + upgradeCost.ToString() + ")";
    }

    public void updateCollectorLoadRateButtonText(float collectorResourceGatherRate, float upgradeCost)
    {
        upgradeCollectorLoadRateButton.GetComponentInChildren<TextMeshProUGUI>().text = collectorResourceGatherRate.ToString() + " (" + upgradeCost.ToString() + ")";
    }

    public void updateCollectorOffloadRateButtonText(float collectorResourceOffloadRate, float upgradeCost)
    {
        upgradeCollectorOffloadRateButton.GetComponentInChildren<TextMeshProUGUI>().text =  collectorResourceOffloadRate.ToString() + " (" + upgradeCost.ToString() + ")";
    }
  
    public void updateCollectorButton(float resourceAmount,float resourceCost){
        UnityEngine.UI.Button button = spawnCollectorButton.GetComponent<UnityEngine.UI.Button>();
        if(resourceAmount < resourceCost)
        {
            spawnCollectorButtonText.text = "Not enough resources (" + resourceCost + ")";
            //spawnCollectorButton getting disabled but visible
            button.interactable = false;
        }
        else
        {
            if(button.interactable == false){
                button.interactable = true;
            }
            spawnCollectorButtonText.text = "Spawn Collector (" + resourceCost + ")";    
        }
    }

}

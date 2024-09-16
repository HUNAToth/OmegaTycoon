using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseBuildingScript : MonoBehaviour
{
    public GameObject collectorPrefab;
    public List<GameObject> collectorsList = new List<GameObject>();
    public CanvasController canvasController;
    public GameObject spawnCollectorButton;

    int resourceCost;

    public GameManager gameManager;
       
    // Start is called before the first frame update
    void Start()
    {  
        // Cache references only if not set in the editor
        if (!canvasController)
            canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();

       
        if (!spawnCollectorButton)
            spawnCollectorButton = GameObject.Find("SpawnCollectorButton");

        if (!gameManager)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        resourceCost = gameManager.calculateCollectorCost();
        
    }

    // Update is called once per frame
    void Update()
    {
        //canvasController.UpdateResourceText(resourceAmount);
        //updateSpawnCollectorButtonText();
    }

    public float GiveResource(float amount)
    {
       if (gameManager.resourceCapacity <= 0)
        {
            return 0;
        }

        // Clamp the amount to be the maximum available capacity
        float givenAmount = Mathf.Min(amount, gameManager.resourceCapacity);
        gameManager.resourceCapacity -= givenAmount;

        return givenAmount;
    }

    public void AddResource(float amount)
    {
        if(gameManager.resourceAmount < gameManager.resourceCapacity)
        {
            gameManager.resourceAmount += amount;
           // Debug.Log("BaseBuilding sent resource " + amount + " to GameManager. New amount in GameManager: " + gameManager.resourceAmount);
        }
    }

    

    public void SpawnResourceCollector(){
        if (gameManager.resourceAmount < resourceCost)
        {
            return;
        }

        // Instantiate the new collector
        GameObject resourceCollector = Instantiate(collectorPrefab, transform.position, Quaternion.identity);
        resourceCollector.transform.parent = transform;

        // Set collector stats
        CollectorController resourceCollectorController = resourceCollector.GetComponent<CollectorController>();
        resourceCollectorController.speed = gameManager.collectorMovementSpeed;
        resourceCollectorController.resourceCapacity = gameManager.collectorResourceCapacity;
        resourceCollectorController.resourceCollectRate = gameManager.collectorResourceGatherRate;
        resourceCollectorController.resourceOffloadRate = gameManager.collectorResourceOffloadRate;

        // Deduct resources
        gameManager.removeResource(resourceCost);

        // Add the new collector to the list
        collectorsList.Add(resourceCollector);
    
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseBuildingScript : MonoBehaviour
{
    public GameObject collectorPrefab;
    public GameObject[] collectors;
    public CanvasController canvasController;
    public GameObject spawnCollectorButton;

    int resourceCost;

    public GameManager gameManager;
       
    // Start is called before the first frame update
    void Start()
    {
        canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        spawnCollectorButton = GameObject.Find("SpawnCollectorButton");
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
        float givenAmount = 0;
        if(gameManager.resourceCapacity <= 0)
        {
            return 0;
        }
          
        if(gameManager.resourceCapacity - amount > 0){
            givenAmount = amount;
            gameManager.resourceCapacity -= amount;
            return givenAmount;
        }
        else
        {
            givenAmount = gameManager.resourceCapacity;
            //what is this line doing?
            //resourceCapacity = 0;
            return givenAmount;
        }
    }

    public void AddResource(float amount)
    {
        if(gameManager.resourceAmount < gameManager.resourceCapacity)
        {
            gameManager.resourceAmount += amount;
            Debug.Log("BaseBuilding sent resource " + amount + " to GameManager. New amount in GameManager: " + gameManager.resourceAmount);
        }
    }

    

    public void SpawnResourceCollector(){
        collectors = GameObject.FindGameObjectsWithTag("Collector");
        int resourceCost = gameManager.calculateCollectorCost();
        Debug.Log("Resource Cost: " + resourceCost + " Resource Amount: " + gameManager.resourceAmount);
        if(gameManager.resourceAmount < resourceCost)
        {
            return;
        }
        GameObject resourceCollector = Instantiate(collectorPrefab, transform.position, Quaternion.identity);
        resourceCollector.transform.parent = transform;
        CollectorController resourceCollectorController = resourceCollector.GetComponent<CollectorController>();
        resourceCollectorController.speed = gameManager.collectorMovementSpeed;
        resourceCollectorController.resourceCapacity = gameManager.collectorResourceCapacity;
        resourceCollectorController.resourceCollectRate = gameManager.collectorResourceGatherRate;
        resourceCollectorController.resourceOffloadRate = gameManager.collectorResourceOffloadRate;

        gameManager.removeResource(resourceCost);
        collectors = GameObject.FindGameObjectsWithTag("Collector");
    }


}

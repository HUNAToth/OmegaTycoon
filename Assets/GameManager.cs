using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BaseBuildingScript baseBuildingScript;
    public CanvasController canvasController;

    public float resourceAmount = 50f;
    public float  resourceCapacity = 1000f;

    public float collectorMovementSpeed = 1f;
    public float collectorMovementSpeedLevel = 1f;
    public float collectorResourceCapacity = 100f;
    public float collectorResourceCapacityLevel = 1f;
    public float collectorResourceAmount = 0f;
    public float collectorResourceGatherRate = 1f;
    public float collectorResourceGatherRateLevel = 1f;
    public float collectorResourceOffloadRate = 1f;
    public float collectorResourceOffloadRateLevel = 1f;

    public float averageResourceCollectionRatePerSecond = 0f;


    public GameObject selectedUnit;


    GameObject[] collectors;
    

    // Start is called before the first frame update
    void Start()
    {
        baseBuildingScript = GameObject.Find("BASE").GetComponent<BaseBuildingScript>();
        canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        canvasController.gameManager = this;
        canvasController.HideCollectorUI();
    }

    // Update is called once per frame
    void Update()
    {
        averageResourceCollectionRatePerSecond = calculateAverageResourceCollectionRatePerSecond();
        HandleUnitSelection();
    }

    public void HandleUnitSelection()
    {
        /*
        if left mouse button is clicked
        check if the object clicked is a collector
        if it is, select the collector
        if not, deselect the collector
        */
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Collector")
                {
                    selectedUnit = hit.collider.gameObject;
                    canvasController.ShowCollectorUI();
                }else{
                    selectedUnit = null;
                    canvasController.HideCollectorUI();
                }
            }
        }
    }

    public void increaseCollectorSpeed()
    {
        if(resourceAvailable(collectorMovementSpeedLevel)){
            collectorMovementSpeed += 0.5f;
            GameObject[] collectors = baseBuildingScript.collectors;
            for(int i = 0; i < collectors.Length; i++)
            {
                collectors[i].GetComponent<CollectorController>().speed = collectorMovementSpeed;
            }
            collectorMovementSpeedLevel += 1;
            removeResource(collectorMovementSpeedLevel * 100);
        }
    }

    public void removeResource(float amount)
    {
        if(resourceAmount >= amount)
        {
            resourceAmount -= amount;
        }
    }

    public void increaseCollectorCapacity()
    {
       if(resourceAvailable(collectorResourceCapacityLevel)){
            collectorResourceCapacity += 30f;
            GameObject[] collectors = baseBuildingScript.collectors;
            for(int i = 0; i < collectors.Length; i++)
            {
                collectors[i].GetComponent<CollectorController>().resourceCapacity = collectorResourceCapacity;
            }
            collectorResourceCapacityLevel += 1;
            removeResource(collectorResourceCapacityLevel * 100);
        }
    }

    public GameObject[] getCollectors(){
        collectors = baseBuildingScript.collectors;
        return collectors;
    }

    public void increaseCollectorLoadRate(){
        if(resourceAvailable(collectorResourceGatherRateLevel)){
            collectorResourceGatherRate += 0.5f;
            GameObject[] collectors = baseBuildingScript.collectors;
            for(int i = 0; i < collectors.Length; i++)
            {
                collectors[i].GetComponent<CollectorController>().resourceCollectRate = collectorResourceGatherRate;
            }
            collectorResourceGatherRateLevel += 1;
            removeResource(collectorResourceGatherRateLevel * 100);
        }
    }

    public void increaseCollectorOffloadRate(){
        if(resourceAvailable(collectorResourceOffloadRateLevel)){
            collectorResourceOffloadRate += 0.5f;
            GameObject[] collectors = baseBuildingScript.collectors;
            for(int i = 0; i < collectors.Length; i++)
            {
                collectors[i].GetComponent<CollectorController>().resourceOffloadRate = collectorResourceOffloadRate;
            }
            collectorResourceOffloadRateLevel += 1;
            removeResource(collectorResourceOffloadRateLevel * 100);
        }
    }

    public bool resourceAvailable(float upgradeLevel)
    {
        float resourceCost = upgradeLevel * 100;
        if(resourceAmount >= resourceCost)
        {
            resourceAmount -= resourceCost;
            return true;
        }
        return false;
    }

    public int calculateUpgradeCost(float upgradeLevel){
        return Mathf.Max(50, (int)upgradeLevel * 100);
    }
    public int calculateCollectorCost(){
        collectors = baseBuildingScript.collectors;
        return  Mathf.Max(50, collectors.Length * 50) ;
    }

    public float calculateAverageResourceCollectionRatePerSecond()
    {
        float totalResourceCollectionRate = 0f;
        collectors = baseBuildingScript.collectors;
        for(int i = 0; i < collectors.Length; i++)
        {
            totalResourceCollectionRate += collectors[i].GetComponent<CollectorController>().resourceCollectRate;
        }
        return totalResourceCollectionRate;
    }
}

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
        if (!Input.GetMouseButtonDown(0)){
            return;
        }
        else{
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

   

    public void removeResource(float amount)
    {
        if(resourceAmount >= amount)
        {
            resourceAmount -= amount;
        }
    }

    public void IncreaseCollectorStat(ref float statValue, ref float statLevel, System.Action<CollectorController, float> applyStat)
    {
        if (resourceAvailable(statLevel))
        {
            statValue += 0.5f;
            foreach (var collector in baseBuildingScript.collectors)
            {
                applyStat(collector.GetComponent<CollectorController>(), statValue);
            }
            statLevel += 1;
        }
    }


     public void increaseCollectorSpeed()
    {
        IncreaseCollectorStat(ref collectorMovementSpeed, ref collectorMovementSpeedLevel, 
        (collector, speed) => collector.speed = speed);
    }

    public void increaseCollectorCapacity()
    {
        //it must be different from the other methods, because the capacity increases faster
       if(resourceAvailable(collectorResourceCapacityLevel)){
                collectorResourceCapacity += 30f;
                foreach (var collector in baseBuildingScript.collectors)
                {
                    collector.GetComponent<CollectorController>().resourceCapacity = collectorResourceCapacity;
                }
                removeResource(collectorResourceCapacityLevel * 100);
                collectorResourceCapacityLevel += 1;
        }
    }

    public void increaseCollectorLoadRate(){
       IncreaseCollectorStat(ref collectorResourceGatherRate, ref collectorResourceGatherRateLevel, 
        (collector, rate) => collector.resourceCollectRate = rate);
    }

    public void increaseCollectorOffloadRate(){
        IncreaseCollectorStat(ref collectorResourceOffloadRate, ref collectorResourceOffloadRateLevel,
        (collector, rate) => collector.resourceOffloadRate = rate);
    }

    
    public GameObject[] getCollectors(){
        collectors = baseBuildingScript.collectors;
        return collectors;
    }

    private Dictionary<float, float> upgradeCostCache = new Dictionary<float, float>();

    public float GetUpgradeCost(float upgradeLevel)
    {
        if (!upgradeCostCache.TryGetValue(upgradeLevel, out var cost))
        {
            cost = Mathf.Max(50, upgradeLevel * 100);
            upgradeCostCache[upgradeLevel] = cost;
        }
        return cost;
    }




    public bool resourceAvailable(float upgradeLevel)
    {
        float resourceCost = GetUpgradeCost(upgradeLevel);
        if(resourceAmount >= resourceCost)
        {
            resourceAmount -= resourceCost;
            return true;
        }else{
            return false;
        }
    }

    public int calculateUpgradeCost(float upgradeLevel){
        return Mathf.Max(50, (int)upgradeLevel * 100);
    }
    public int calculateCollectorCost(){
        collectors = baseBuildingScript.collectors;
        return  Mathf.Max(50, collectors.Length * 50) ;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorController : MonoBehaviour
{
    //collector is an object that moves between base, and the closest resource
    //closest resource is determined by the distance between the collector and the resource
    //this distance is calculated by the FindClosestResource() method
    //collector will move to the closest resource, wait a while, and then return to base
    //collector will have a speed variable, and a rigidbody component
    //collector will have a reference to the base


    GameObject baseBuilding;
    public float speed = 1f;
    public Rigidbody collectorRb;
    public GameObject targetResource;
    public bool isMovingToResource = false;

    public bool isCollecting = false;
    public bool isOffloading = false;

    public float resourceCollectRate = 1f;
    public float resourceOffloadRate = 1f;

    public float resourceCollected = 0f;
    public float collectTime = 5f;
    public float collectTimer = 0f;
    public float offloadingTime = 5f;
    private float offloadingTimer = 0f;
    public float resourceCapacity = 100f;
    public bool isSelected = false;
    public bool onBase = false;

    public Color highlightColor = Color.yellow;

    public ParticleSystem miningEffect;

    private Renderer selfRenderer;

    public Material selfMaterial;
    public Material selfSelectedMaterial;

    public GameObject shipPrefab;


    // Start is called before the first frame update
    void Start()
    {
        collectorRb = GetComponent<Rigidbody>();
        baseBuilding = GameObject.Find("BASE");
        gameObject.name = GetInitShipName();
        miningEffect = GetComponentInChildren<ParticleSystem>();
        miningEffect.Stop();
        selfRenderer = transform.Find("Spacefrigate").GetComponent<Renderer>();
        selfMaterial = selfRenderer.material;
        shipPrefab = transform.Find("Spacefrigate").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        selfSelectCheck();
        if (isCollecting)
        {
            CollectResource(targetResource.GetComponentInParent<ResourcePointScript>());
            if (!miningEffect.isPlaying)
            {
                Debug.Log("Mining effect started");
                miningEffect.Play();
            }
        }
        if (isOffloading)
        {
            OffLoadResource();
        }

        if (isMovingToResource)
        {
            miningEffect.Stop();
            targetResource = FindClosestResource();
            if (targetResource != null)
            {
                MoveToResource();
            }
        }
        else
        {
            miningEffect.Stop();
            MoveToBase();
        }
    }

    void selfSelectCheck()
    {

        if (baseBuilding.GetComponent<BaseBuildingScript>().gameManager.selectedUnit != null &&
            (baseBuilding.GetComponent<BaseBuildingScript>().gameManager.selectedUnit.name == this.name))
        {
            isSelected = true;
            shipPrefab.GetComponent<Renderer>().material = selfSelectedMaterial;
        }
        else
        {
            isSelected = false;
            shipPrefab.GetComponent<Renderer>().material = selfMaterial;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            ResourcePointScript resourcePoint = other.GetComponent<ResourcePointScript>();
            if (resourcePoint.isHarvested &&
                resourcePoint.currentCollector != this)
            {
                return;
            }
            else
            {

                CollectResource(resourcePoint);
            }

        }
        if (other.CompareTag("Base"))
        {
            if (resourceCollected > 0)
            {
                OffLoadResource();
            }
            else
            {
                isMovingToResource = true;
            }

        }
    }


    //find the closest resource to the collector
    //the resouce is determined by the distance between the collector and the resource
    GameObject FindClosestResource()
    {
        GameObject[] resources;
        resources = GameObject.FindGameObjectsWithTag("Resource");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject resource in resources)
        {
            ResourcePointScript resourcePoint = resource.GetComponentInParent<ResourcePointScript>();
            if (resourcePoint.isHarvested &&
               resourcePoint.currentCollector != this)
            {
                continue;
            }
            Vector3 diff = resource.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = resource;
                distance = curDistance;
            }
        }
        // Debug.Log("Closest resource found+ " + closest);
        return closest;
    }

    //moves the collector to the closest resource
    void MoveToResource()
    {
        //Debug.Log("Moving to resource");
        MoveToPoint(targetResource.transform.position);
    }

    void MoveToBase()
    {
        MoveToPoint(baseBuilding.transform.position);
    }

    void MoveToPoint(Vector3 point)
    {
        transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
        transform.LookAt(point);
    }

    void CollectResource(ResourcePointScript resourcePoint)
    {
        //  Debug.Log("Collecting resource");
        isCollecting = true;
        miningEffect.Play();
        resourcePoint.isHarvested = true;
        resourcePoint.currentCollector = this;
        if (resourceCapacity >= resourceCollected)
        {
            collectTimer += Time.deltaTime;
            if (collectTimer >= collectTime)
            {
                collectTimer = 0;
                float resourceAmount = resourcePoint.resourceAmount;
                resourceAmount -= resourceAmount - resourceCollectRate >= 0 ? resourceCollectRate : 0;
                float gotAmount = resourcePoint.GiveResource(resourceCollectRate);
                resourceCollected += gotAmount;
            }
        }
        if (resourceCapacity <= resourceCollected)
        {
            Debug.Log("Resource capacity reached");
            resourcePoint.isHarvested = false;
            resourcePoint.currentCollector = null;
            isMovingToResource = false;
            isCollecting = false;
        }
    }

    void OffLoadResource()
    {
        //   Debug.Log("Offloading resource");
        isOffloading = true;
        GameManager gameManager = baseBuilding.GetComponent<BaseBuildingScript>().gameManager;
        if (resourceCollected > 0)
        {
            offloadingTimer += Time.deltaTime;
            if (offloadingTimer >= offloadingTime)
            {
                offloadingTimer = 0;
                baseBuilding.GetComponent<BaseBuildingScript>().AddResource(resourceOffloadRate);
                // Debug.Log("Resource offloaded, new amount in GameManager: " + gameManager.resourceAmount);
                resourceCollected -= resourceOffloadRate;
            }
        }
        if (resourceCollected <= 0)
        {
            isOffloading = false;
            isMovingToResource = true;
            resourceCollected = 0;
        }
    }

    public void AddResource(float amount)
    {
        resourceCollected += amount;
    }


    public string GetInitShipName()
    {
        string[] shipnames =
        new string[] {
            "The Flying Dutchman",
            "The Black Pearl",
            "The Jolly Roger",
            "The Queen Anne's Revenge",
            "The Mayflower",
            "The Bounty",
            "The Endurance",
            "The Titanic",
            "The Lusitania",
            "The Britannic",
            "The Enterprise",
            "The Millennium Falcon",
            "The Serenity",
            "The Bebop",
            "The Normandy",
            "The Galactica",
            "The Defiant",
            "The Discovery",
            "The Nostromo",
            "The Rocinante"};
        return shipnames[Random.Range(0, shipnames.Length)];
    }

}

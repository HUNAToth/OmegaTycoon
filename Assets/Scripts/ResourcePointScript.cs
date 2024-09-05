using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePointScript : MonoBehaviour
{
    [SerializeField] public float resourceAmount = 0f;
    [SerializeField] private float maxResourceCapacity = 100f;
    [SerializeField] private float resourceRegenRate = 1.0f;
    [SerializeField] private float resourceRegenTime = 5.0f;
    [SerializeField] private float resourceRegenTimer = 0.0f;
    [SerializeField] public bool isHarvested = false;
    [SerializeField] public CollectorController currentCollector = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RegenResource();
    }

    public float GiveResource( float amount)
    {
        float givenAmount = 0;
        if(resourceAmount <= 0)
        {
            return 0;
        }
          
        if(resourceAmount - amount > 0){
            givenAmount = amount;
            resourceAmount -= amount;
            return givenAmount;
        }
        else
        {
            givenAmount = resourceAmount;
            resourceAmount = 0;
            return givenAmount;
        }
    }

    public void RegenResource()
    {
        if (resourceAmount < maxResourceCapacity)
        {
            resourceRegenTimer += Time.deltaTime;
            if (resourceRegenTimer >= resourceRegenTime)
            {
                resourceAmount += resourceRegenRate;
                resourceRegenTimer = 0.0f;
            }
        }
    }   
}

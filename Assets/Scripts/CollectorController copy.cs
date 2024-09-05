/*using System.Collections;
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
    bool isMovingToResource = false;
    // Start is called before the first frame update
    void Start()
    {
        collectorRb = GetComponent<Rigidbody>();
        baseBuilding = GameObject.Find("BASE");
    }

    // Update is called once per frame
    void Update()
    { 
        if(targetResource == null)
        {
            targetResource = FindClosestResource();
        }
    }

    private void FixedUpdate()
    {
        if (isMovingToResource)
        {
            MoveToResource();
        }
        else
        {
            MoveToBase();
        }

       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            isMovingToResource = false;
        }
        if(other.CompareTag("Base"))
        {
            isMovingToResource = true;
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
            Vector3 diff = resource.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = resource;
                distance = curDistance;
            }
        }
        return closest;
    }

    //moves the collector to the closest resource
    
    //the resource can be obstructed by other objects, or terrain, 
    //so the collector will move to the closest point it can reach, 
    //or try to circle around the obstacle, if it is possible, 
    //this increase the distance between the collector and the resource
    void MoveToResource()
    {
        //direction of closest resource
        Vector3 direction = targetResource.transform.position - transform.position;
        //if there is no obstactle between the collector and the resource,
        // move to the resource
        //else circle around the obstacle
        Vector3 offset = transform.position - targetResource.transform.position;
        Physics.Raycast(transform.position, direction, out RaycastHit hit, direction.magnitude) ;
        if(hit.collider.gameObject.CompareTag("Resource")){
            collectorRb.AddForce(direction * speed);
        }
        else
        {
            //circle around the obstacle
            Debug.Log("Circling around the obstacle");
            Vector3 direction2 = Vector3.Cross(offset, Vector3.up);
            collectorRb.AddForce(direction2 * speed);
        } 

//if the collector is in the vicinity of the resource, it stops
        if(direction.magnitude < 3)
        {
            collectorRb.velocity = Vector3.zero;
            isMovingToResource = false;
            targetResource = null;
        }
    }

    void MoveToBase()
    {

        Vector3 direction = baseBuilding.transform.position - transform.position;
        //if the collector is in the vicinity of the base, it stops
        if(direction.magnitude < 3)
        {
            collectorRb.velocity = Vector3.zero;
            isMovingToResource = true;
            targetResource = FindClosestResource();
        }
        else{
            collectorRb.AddForce(direction * speed);
        }
    }

    void MoveToPoint(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        collectorRb.AddForce(direction * speed);
    }
    
}
*/
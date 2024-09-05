using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    //this script will move the gameobject in a circular path, ű
    //the object will rotate around the focal point
    //the object will keep looking at the focal point
    //the object will move in a circular path around the focal point, keeping distance
    
    public float speed = 1f;
    public Rigidbody asteroidRb;
    public GameObject focalPoint;
    public float rotateSpeed = 20.0f;

    public float distance = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        asteroidRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the object around the focal point
        transform.RotateAround(focalPoint.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        //move the object in a circular path around the focal point
        Vector3 offset = transform.position - focalPoint.transform.position;
        Vector3 direction = Vector3.Cross(offset, Vector3.up);
        asteroidRb.AddForce(direction * speed);
        //keep the object at a certain distance from the focal point
        if (offset.magnitude > distance)
        {
            asteroidRb.AddForce(-direction * speed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 20;
    float horizontalInput ;
    float verticalInput;
    float scrollDirection;

    GameObject cameraObject;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        scrollDirection = Input.mouseScrollDelta.y;
        cameraObject = GameObject.Find("Main Camera");
        cam = cameraObject.GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //if player presses the left/right arrow keys, rotate the camera left or right
        //between 0 and 181 degrees
        horizontalInput = Input.GetAxis("Horizontal");
        if(
            transform.rotation.y < 0.25f && horizontalInput > 0
            || 
            transform.rotation.y > -0.25f && horizontalInput < 0
        ){
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }
       
       //if player presses the up arrow/down key, move the camera up or down

        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * verticalInput * rotationSpeed * Time.deltaTime);

        //watch the mouse scroll wheel delta, zoom in or out
        //increase/decrease the field of view of the camera

        scrollDirection = Input.mouseScrollDelta.y;

        if (cam.fieldOfView <= 100 && scrollDirection > 0){
            cam.fieldOfView += Input.mouseScrollDelta.y*rotationSpeed*Time.deltaTime*10.0f;
        }
        if (cam.fieldOfView >= 30 && scrollDirection < 0){
            cam.fieldOfView += Input.mouseScrollDelta.y*rotationSpeed*Time.deltaTime*10.0f;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.003f;
    private Rigidbody enemyRb;
    private GameObject player;

    Vector3 lookDirection;

    private float timeDelay = 0;
    private float timeDelayMax = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        timeDelay+=Time.deltaTime;
        if(timeDelay >= timeDelayMax){
            lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);
            timeDelay = 0;
        }        
        if(transform.position.y < -15){
            Destroy(gameObject);
        }
    }
}

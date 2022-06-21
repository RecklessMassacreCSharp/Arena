using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public GameObject playerCam;
    public Rigidbody playerRB;
    public float runningSpeed;
    public float walkingSpeed;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    
}

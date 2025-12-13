using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    // Start is called before the first frame update  
    public Transform hatch;
    public float speed = 10;
    private Vector3 direction; // Declare the field without initialization

    void Start()
    {
        // Initialize the direction in the Start method to avoid referencing non-static fields
        direction = -hatch.transform.up;
    }

    // Update is called once per frame  
    void Update()
    {
        transform.Rotate(direction, speed * Time.deltaTime); // Fixed Transform to lowercase transform  
    }

    // Update is called once per frame///
}

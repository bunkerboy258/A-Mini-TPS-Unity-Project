using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grindcontrol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zombie"))
        {
            other.GetComponent<zombiecontrol>().BeHit(1000);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrelcontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public hatchcontrol a;
    private Vector3 des;
    void Start()
    {
        a = GetComponent<hatchcontrol>();
        des = a.rayEndPoint;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(des);
    }
}

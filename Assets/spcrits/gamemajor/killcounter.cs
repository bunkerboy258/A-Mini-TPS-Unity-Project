using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter : MonoBehaviour
{
    // Start is called before the first frame update
    public int gzkillcount;
    public int rescuenum;
    public static counter instance { get; private set; }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addgzkill()
    {
        gzkillcount++;
    }

    public void addrescue()
    {
        rescuenum++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tankhealthslider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthslider;
    public TankControl tank;
    void Start()
    {
        healthslider.maxValue = tank.maxhealth;
        healthslider.value = tank.maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = tank.health;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSwitch : MonoBehaviour
{
    public bool switchOn;
    public bool canChange;

    public Light indicatorLight;

    void Update()
    {
        indicatorLight.color = switchOn ? Color.green : Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            canChange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            canChange = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    Switcher[] objectsToSwitch;
   
    void Start()
    {
        objectsToSwitch = FindObjectsOfType(typeof(Switcher)) as Switcher[];
    }

    public void SwitchObjects()
    {
        Debug.Log("Switching Positions");

        foreach(var obj in objectsToSwitch)
        {
            obj.SwitchPos();
        }
    }
}

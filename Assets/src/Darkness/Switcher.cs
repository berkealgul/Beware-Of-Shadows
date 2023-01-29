using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public Vector2 targetPos;
    
    public void SwitchPos()
    {
        transform.position = targetPos;
    }
}

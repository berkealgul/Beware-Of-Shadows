using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public List<Vector2> targetPoses;

    int idx = 0;

    void Start()
    {
        targetPoses.Add(transform.position);    
    }

    public void SwitchPos()
    {
        transform.position = targetPoses[idx];
        idx = (idx + 1) % targetPoses.Count;
    }
}

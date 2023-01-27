using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    static readonly float maxEnergy = 100;
    static readonly float energyDecay = 1; // per second;

    float energy;

    void Start()
    {
        energy = maxEnergy;
    }

    void Update()
    {
        energy = Mathf.Max(energy - Time.deltaTime*energyDecay, 0);
    }

    public void StealEnergy(float stolenAmount)
    {
        energy = Mathf.Max(energy - stolenAmount, 0);
    }

    public float GetEnergy() { return energy; }
}

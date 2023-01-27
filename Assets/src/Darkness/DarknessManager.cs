using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessManager : MonoBehaviour
{
    // seconds
    [Min(1)]
    public float timeBetweenPlayerAttacks = 10;
    [Min(1)]
    public float timeBetweenGenAttacks = 20;

    float timeToPlayerAttack;
    float timeToGenAttack;

    GameObject[] generators;

    float doomMeter = 1;
    float difficulty = 1;
    float maxEnergy;

    bool active = false;

    void Start() 
    {
        init();
    }

    void Update()
    {
        timeToGenAttack -= Time.deltaTime * difficulty;
        timeToPlayerAttack -= Time.deltaTime * difficulty;

        // attack to generators
        if(timeToGenAttack <= 0)
        {

        }

        // attack to player
        if (timeToPlayerAttack <= 0)
        {

        }

    }

    void init()
    {
        generators = GameObject.FindGameObjectsWithTag("Generator");
        timeToGenAttack = timeBetweenGenAttacks;
        timeToPlayerAttack = timeBetweenPlayerAttacks;
        maxEnergy = CalculateTotalEnergy();
        Activate();
    }

    float CalculateTotalEnergy()
    {
        float totalEnergy = 0;

        foreach(var gen in generators)
        {
            totalEnergy += gen.GetComponent<Generator>().GetEnergy();
        }

        return totalEnergy;
    }

    public float CalculateDoomPercentage()
    {
        return CalculateTotalEnergy() / maxEnergy;
    }

    public void Activate() { active = true; }
    public void Deactivate() { active = false; }

}

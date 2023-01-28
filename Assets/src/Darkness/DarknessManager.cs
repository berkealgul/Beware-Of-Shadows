using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessManager : MonoBehaviour
{
    // seconds
    [Min(1)]
    public float timeBetweenPlayerAttacks = 1;
    [Min(1)]
    public float timeBetweenGenAttacks = 20;

    [Min(1)]
    public float spawnRadius = 5;

    public GameObject darknessPrefab;

    GameObject[] generators;

    float timeToPlayerAttack;
    float timeToGenAttack;

    float doomMeter = 1;
    float difficulty = 1;
    float maxEnergy;

    int maxSpawn = 3;
    float spawnArcDegrees = 60;

    bool active = false;

    void Start() 
    {
        init();
    }

    void Update()
    {
        if(!active) { return; }

        timeToGenAttack -= Time.deltaTime;
        timeToPlayerAttack -= Time.deltaTime;

        // attack to generators
        if(timeToGenAttack <= 0)
        {
            int i = Random.Range(0, generators.Length);
            AttackToGenerator(generators[i]);
            //Debug.Log("Attack gen");
            timeToGenAttack = timeBetweenGenAttacks;
        }

        // attack to player
        if (timeToPlayerAttack <= 0)
        {
            timeToPlayerAttack = timeBetweenPlayerAttacks;
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

    void AttackToGenerator(GameObject gen)
    {
        int spawns = Random.Range(1, maxSpawn + 1);
        Vector2 spawnDir = gen.transform.position.normalized;

        for(int i = 0; i < spawns; i++)
        {
            float angle = Random.Range(-spawnArcDegrees / 2, spawnArcDegrees / 2);

            Vector2 spawnPos = RotateVector(spawnDir, angle) * spawnRadius;
            Quaternion q = new Quaternion();

            GameObject instance = Instantiate(darknessPrefab, spawnPos, q);
            instance.GetComponent<Darkness>().SetTarget(gen);
        }
    }

    Vector2 RotateVector(Vector2 v, float angle)
    {
        float _x = v.x;
        float _y = v.y;

        float _angle = angle * Mathf.Deg2Rad;
        float _cos = Mathf.Cos(_angle);
        float _sin = Mathf.Sin(_angle);

        float _x2 = _x * _cos - _y * _sin;
        float _y2 = _x * _sin + _y * _cos;

        return new Vector2(_x2, _y2); 
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

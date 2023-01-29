using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Sprite[] sprites;

    float maxEnergy = 100;
    float energyDecay = 2; // per second;
    float fixEnergy = 20;
    float energy;

    bool broken = false;

    void Start()
    {
        energy = maxEnergy;
    }

    void Update()
    {
        energy = Mathf.Max(energy - Time.unscaledDeltaTime*energyDecay, 0);
        UpdateSprite();
    }

    void UpdateSprite()
    {
        int idx = 0;
        // 0 25 50 75 100 broken normal
        if (energy > maxEnergy - 10) { idx = 4; }
        else if (energy > 75) { idx = 3; }
        else if (energy > 50) { idx = 2; }
        else if (energy > 25) { idx = 1; }
        else { idx = 0; }

        if(energy == 0 && !broken)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[5];
            broken = true;
        }
        else if(energy > 0 && broken)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[6];
            broken = false;
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[idx];
    }

    public void Fix()
    {
        //Debug.Log("FIX");
        energy = Mathf.Min(energy+fixEnergy, maxEnergy);
    }

    public void StealEnergy(float stolenAmount)
    {
        energy = Mathf.Max(energy - stolenAmount, 0);
    }

    public float GetEnergy() { return energy; }

    public bool Broken() { return broken; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightComponent : MonoBehaviour
{
    [Min(1)]
    public float hitDamage = 5;

    bool active = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!active) { return; }

        if(collision.tag == "Darkness")
        {
            collision.GetComponent<Darkness>().Damage(hitDamage);
        }
    }

    public void Activate() { active = true; }
    public void Deactivate() { active = false; }
}

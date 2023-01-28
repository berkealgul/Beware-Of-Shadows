using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Min(0)]
    public float delayToInteract = 1;

    float timeToInteract;

    void Start()
    {
        Init();
    }

    void Init()
    {
        timeToInteract = 0;
    }

    void Update()
    {
        timeToInteract -= Time.unscaledDeltaTime;

        if(Input.GetAxisRaw("Interact") == 1)
        {
            Interact();
        }

    }

    void Interact()
    {
        if(timeToInteract > 0) { return; }

        timeToInteract = delayToInteract;

        var casts = Physics2D.RaycastAll(gameObject.transform.position, new Vector2(1, 0), 1);
        //Debug.DrawRay(gameObject.transform.position, new Vector2(1, 0), Color.red, 1);
        foreach (var c in casts)
        {
            if (c.collider.tag == "Generator")
            {
                c.collider.GetComponent<Generator>().Fix();
            }

            if (c.collider.tag == "Crate")
            {
                Debug.Log("Hide");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Min(0)]
    public float delayToInteract = 1;

    public Sprite[] sprites;

    float timeToInteract;

    enum DIR { LEFT, RIGHT, UP, DOWN };

    DIR dir;
    bool hidden;
    GameObject crate;

    // gen-fix  hide move
    AudioSource[] sfxs;

    void Start()
    {
        Init();
        sfxs = GetComponents<AudioSource>();
    }

    void Init()
    {
        timeToInteract = 0;
        dir = DIR.RIGHT;
        hidden = false;
    }

    void Update()
    {
        timeToInteract -= Time.unscaledDeltaTime;

        if(Input.GetAxisRaw("Interact") == 1)
        {
            Interact();
        }

        // dont update direction if hidden
        if(hidden) { return; }

        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            dir = DIR.RIGHT;
            SetRotation(0);
            UpdateSprite();
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            dir = DIR.LEFT;
            SetRotation(180);
            UpdateSprite();
        }
        else if (Input.GetAxisRaw("Vertical") == 1)
        {
            dir = DIR.UP;
            SetRotation(90);
            UpdateSprite();
        }
        else if(Input.GetAxisRaw("Vertical") == -1)
        {
            dir = DIR.DOWN;
            SetRotation(270);
            UpdateSprite();
        }
    }

    void SetRotation(float degrees)
    {
        Transform t = transform.GetChild(0);
        float x = t.rotation.eulerAngles.x;
        float y = t.rotation.eulerAngles.y;
        t.rotation = Quaternion.Euler(x, y, degrees);
    }

    void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[(int)dir];
    }

    void Interact()
    {
        if(timeToInteract > 0) { return; }

        timeToInteract = delayToInteract;

        if(hidden)
        {
            Reveal();
            return;
        }

        Vector2 dirV = new Vector2(0, 0);

        switch(dir)
        {
            case DIR.RIGHT:
                dirV = new Vector2(1, 0);
                break;
            case DIR.LEFT:
                dirV = new Vector2(-1, 0);
                break;
            case DIR.UP:
                dirV = new Vector2(0, 1);
                break;
            case DIR.DOWN:
                dirV = new Vector2(0, -1);
                break;
        }
        
        var casts = Physics2D.RaycastAll(gameObject.transform.position, dirV, 1);
        //Debug.DrawRay(gameObject.transform.position, dirV, Color.red, 0.2f);
        foreach (var c in casts)
        {
            if (c.collider.tag == "Generator")
            {
                c.collider.GetComponent<Generator>().Fix();
                sfxs[0].Play();
            }

            if (c.collider.tag == "Crate")
            {
                Hide(c.collider.gameObject);
                sfxs[1].Play();
            }
        }
    }

    void Hide(GameObject crate_)
    {
        crate = crate_;
        GetComponent<PlayerMovementComponent>().Freeze(true); //freeze player
        GetComponent<SpriteRenderer>().enabled = false; //disable rendering of player
        transform.GetChild(0).GetComponent<FlashlightComponent>().Deactivate(); //disable flashlight
        crate.GetComponent<Crate>().SetSpriteHasPlayer(); // change crate sprite
        hidden = true;
    }
    
    // Inverse of Hide()
    void Reveal()
    {
        GetComponent<PlayerMovementComponent>().Freeze(false);
        GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<FlashlightComponent>().Activate();
        crate.GetComponent<Crate>().SetSpriteNormal();
        hidden = false;
    }

    public void PlayWalkSound()
    {
        sfxs[2].Play();
    }

    public void SetHidden(bool h) { hidden = h; }

    public bool Hidden() { return hidden; }

}

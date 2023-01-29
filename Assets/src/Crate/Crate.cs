using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    // has player, normal
    public Sprite[] sprites;

    public void SetSpriteHasPlayer()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    public void SetSpriteNormal()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
}

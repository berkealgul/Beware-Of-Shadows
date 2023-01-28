using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public AnimationCurve movementCurve;

    public int movementLength = 5; 

    [Range(0.1f , 1)]
    public float movementDuration = 0.2f; // in seconds

    public List<string> walkableTags; 

    bool canMove = true;
    float x_dir = 0;
    float y_dir = 0;
    float anim_dir = 1;

    void Update()
    {
        if(!canMove) { return; }

        x_dir = Input.GetAxisRaw("Horizontal");

        if(x_dir != 0)
        {
            y_dir = 0;
            StartCoroutine(AnimateMovement());
            return;
        }

        y_dir = Input.GetAxisRaw("Vertical");

        if (y_dir != 0)
        {
            x_dir = 0;
            StartCoroutine(AnimateMovement());
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!walkableTags.Contains(collision.tag))
        {
            anim_dir = -1;
        }
    }

    IEnumerator AnimateMovement()
    {
        canMove = false;
        anim_dir = 1;

        float time = 0;
        float start_x = transform.position.x;
        float start_y = transform.position.y;

        while(time < movementDuration && time >= 0)
        {
            float x = Mathf.Lerp(start_x, start_x + movementLength * x_dir, movementCurve.Evaluate(time / movementDuration));
            float y = Mathf.Lerp(start_y, start_y + movementLength * y_dir, movementCurve.Evaluate(time / movementDuration));
            transform.position = new Vector3(x, y, 0);
            time += Time.unscaledDeltaTime * anim_dir;
            yield return null;
        }

        canMove = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public AnimationCurve movementCurve;

    public int movementLength = 5; 

    [Range(0.1f , 1)]
    public float movementDuration = 0.2f; // in seconds

    public List<string> blockTags; 

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
        if(blockTags.Contains(collision.tag))
        {
            anim_dir = -1;
        }
    }

    IEnumerator AnimateMovement()
    {
        if (CanWalk())
        {
            canMove = false;
            anim_dir = 1;

            float time = 0;
            float start_x = transform.position.x;
            float start_y = transform.position.y;
            float x = 0;
            float y = 0;

            while (time < movementDuration && time >= 0)
            {
                x = Mathf.Lerp(start_x, start_x + movementLength * x_dir, movementCurve.Evaluate(time / movementDuration));
                y = Mathf.Lerp(start_y, start_y + movementLength * y_dir, movementCurve.Evaluate(time / movementDuration));

                transform.position = new Vector3(x, y, 0);
                time += Time.unscaledDeltaTime * anim_dir;
                yield return null;
            }

            //snap to grid (.5)
            x = Mathf.Round(x + 0.5f) - 0.5f;
            y = Mathf.Round(y + 0.5f) - 0.5f;
            transform.position = new Vector3(x, y, 0);

            canMove = true;
        }
    }

    bool CanWalk()
    {
        var casts = Physics2D.RaycastAll(gameObject.transform.position, new Vector2(x_dir, y_dir), movementLength);

        foreach (var c in casts)
        {
            if (blockTags.Contains(c.collider.tag))
            {
                return false;
            }
        }

        return true;
    }

    // freezes player if given bool is true
    public void Freeze(bool f) { canMove = !f; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public AnimationCurve movementCurve;

    public int movementLength = 5; 

    [Range(0.1f , 1)]
    public float movementDuration = 0.2f; // in seconds

    bool canMove = true;
    float x_dir = 0;
    float y_dir = 0;


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

    IEnumerator AnimateMovement()
    {
        canMove = false;

        float time = 0;
        float start_x = transform.position.x;
        float start_y = transform.position.y;

        while(time < movementDuration)
        {
            float x = Mathf.Lerp(start_x, start_x + movementLength * x_dir, movementCurve.Evaluate(time / movementDuration));
            float y = Mathf.Lerp(start_y, start_y + movementLength * y_dir, movementCurve.Evaluate(time / movementDuration));
            transform.position = new Vector3(x, y, 0);
            time += Time.deltaTime;
            yield return null;
        }

        canMove = true;
    }
}

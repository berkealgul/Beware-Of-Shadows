using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDarkness : MonoBehaviour
{
    GameObject player;
    Vector3 center;
    float speed;
    float angle;
    float rad;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(player);
    }

    private void Start()
    {
        transform.position = fromPolar();
    }

    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(a, Vector3.forward);

        angle += speed * Time.unscaledDeltaTime * Mathf.Deg2Rad;
        transform.position = fromPolar();
    }

    Vector3 fromPolar()
    {
        float x = Mathf.Cos(angle) * rad;
        float y = Mathf.Sin(angle) * rad;
        return new Vector3(x, y, 0) + center;
    }

    public void SetPolar(float a, float r)
    {
        angle = a;
        rad = r;
    }

    public void SetSpeed(float s) { speed = s; }

    public void SetCenter(Vector3 c) { center = c; }
}

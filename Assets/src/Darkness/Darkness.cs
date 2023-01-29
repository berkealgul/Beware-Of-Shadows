using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    enum State { IDLE, ATTACK, REACH, RETURN, DESTROY, ESCAPE };

    static readonly float distanceToReach = 0.01f; // meters

    [Min(0)]
    public float damageDelay = 0.2f;
    [Min(0)]
    public float escapeDelay = 1f;
    [Min(0)]
    public float v = 5;
    [Min(0)]
    public float energySteal = 5;
    [Min(1)]
    public float hp = 100;

    Vector3 targetPos;
    Vector3 spawnPos;

    GameObject targetObject;
    State state;

    bool attacked = false;
    float timeToDmg = 0;
    float timeToEscape = 0;

    private void Awake()
    {
        spawnPos = gameObject.transform.position;
        state = State.IDLE;
        attacked = false;
    }

    void Update()
    {
        switch(state)
        {
            case State.REACH:
                ExecuteReach();
                break;
            case State.ATTACK:
                ExecuteAttack();
                break;
            case State.RETURN:
                ExecuteReturn();
                break;
            case State.DESTROY:
                ExecuteDestroy();
                break;
            case State.ESCAPE:
                ExecuteEscape();
                break;
        }
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
        targetPos = target.transform.position;
        state = State.REACH;
    }

    public void Damage(float dmg)
    {
        timeToDmg -= Time.unscaledTime;

        if (timeToDmg > 0) { return; }

        timeToDmg = damageDelay;
        timeToEscape = escapeDelay;
        hp -= dmg;
        state = State.ESCAPE;

        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ExecuteDestroy()
    {
        //Debug.Log("Destroyed");
        Destroy(gameObject);
    }

    void ExecuteReturn()
    {
        targetPos = spawnPos;
        state = State.REACH;
    }

    void ExecuteEscape()
    {
        timeToEscape -= Time.unscaledDeltaTime;

        if(timeToEscape <= 0)
        {
            state = State.REACH;
            return;
        }

        Vector3 dis = spawnPos - transform.position;
        transform.position += dis.normalized * v * Time.unscaledDeltaTime;

        // kill enemies earlier
        if (dis.magnitude <= distanceToReach * 3) { state = State.DESTROY; }
    }

    void ExecuteReach()
    {
        Vector3 dis = targetPos - transform.position;

        if(dis.magnitude <= distanceToReach)
        {
            if(attacked) 
                state = State.DESTROY;
            else
                state = State.ATTACK;

            return;
        }

        transform.position += dis.normalized * v * Time.unscaledDeltaTime;
    }

    void ExecuteAttack()
    {
        targetObject.GetComponent<Generator>().StealEnergy(energySteal);
        state = State.RETURN;
        attacked = true;
    }
}

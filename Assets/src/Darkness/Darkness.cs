using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    enum State { IDLE, ATTACK, REACH, RETURN, DESTROY };

    static readonly float distanceToReach = 0.01f; // meters

    [Min(0)]
    public float v = 5;

    public GameObject targetObject;

    Vector3 targetPos;
    Vector3 spawnPos;
    State state;
    bool attacked = false;

    void Start()
    {
        spawnPos = transform.position;
        targetPos = targetObject.transform.position;
        state = State.REACH;
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
        }
    }

    void ExecuteDestroy()
    {
        Debug.Log("Destroyed");
    }

    void ExecuteReturn()
    {
        targetPos = spawnPos;
        state = State.REACH;
    }

    void ExecuteReach()
    {
        Vector3 dis = targetPos - transform.position;

        if(dis.magnitude <= distanceToReach)
        {
            if(attacked) // temporary
                state = State.DESTROY;
            else
                state = State.ATTACK;

            return;
        }

        transform.position += dis.normalized * v * Time.deltaTime;
    }

    void ExecuteAttack()
    {
        Debug.Log("Attack");
        targetObject.GetComponent<Generator>().StealEnergy(50);
        state = State.RETURN;
        attacked = true;
    }
}

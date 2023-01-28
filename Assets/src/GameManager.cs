using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    float gameStartTime;

    DarknessManager dm;




    void Start()
    {
        dm = GetComponent<DarknessManager>();
    }

    void Update()
    {

    }

    void StartGame()
    {
        gameStartTime = Time.time;
    }

    bool isGameOver() 
    {
        return dm.CalculateDoomPercentage() <= 0;
    }
}

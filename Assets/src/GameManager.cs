using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Range(1, 10)]
    public float timeScale = 1;
    [Min(60)]
    public float totalPlayingTime = 600; 

    float playingTime;
    float doomPer;

    DarknessManager dm;

    void Start()
    {
        dm = GetComponent<DarknessManager>();
    }

    void Update()
    {
        doomPer = dm.CalculateDoomPercentage();

        playingTime += Time.deltaTime;

        int mins = (int)playingTime / 60;

        //Debug.Log(playingTime);
        //Debug.Log(mins);

        if(Lose())
        {
            //Debug.Log("LOSE");
        }
        
        if(Win())
        {
           // Debug.Log("Win");
        }
        else
        {
            //Debug.Log(doomPer);
        }

        Time.timeScale = timeScale; // will be carried to Start()
    }

    bool Lose() 
    {
        return doomPer <= 0;
    }

    bool Win()
    {
        return playingTime >= totalPlayingTime;
    }
}

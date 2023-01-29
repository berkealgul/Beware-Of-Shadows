using System;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    [Range(1, 10)]
    public float timeScale = 1;
    [Min(60)]
    public float totalPlayingTime = 600;

    [Header("UI")]
    public GameObject doomBar;
    public GameObject timeText;
    public GameObject statusText;

    float playingTime;
    float doomPer;

    DarknessManager dm;
    ProgressBar pb;
    TMP_Text tmt;
    TMP_Text st;

    AudioSource music_fs;
    AudioSource music_ss;

    void Start()
    {
        dm = GetComponent<DarknessManager>();
        pb = doomBar.GetComponent<ProgressBar>();
        tmt = timeText.GetComponent<TMP_Text>();
        st = statusText.GetComponent<TMP_Text>();
        st.enabled = false;
        Time.timeScale = timeScale;
    }

    void Update()
    {
        doomPer = dm.CalculateDoomPercentage();
        playingTime += Time.deltaTime;

        pb.SetPer(doomPer);
        tmt.text = GetTimeString();

        //Debug.Log(playingTime);
        //Debug.Log(mins);

        if(Lose())
        {
            st.text = "LOSE";
            st.enabled = true;
        }
        
        if(Win())
        {
           st.text = "Win";
           st.enabled = true;
        }
    }

    String GetTimeString()
    {
        int virMins = (int)playingTime / 60;
        int virSecs = (int)playingTime % 60;

        return virMins.ToString() + " : " + virSecs.ToString().PadLeft(2, '0');
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

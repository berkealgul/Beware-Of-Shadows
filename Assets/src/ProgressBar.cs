using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public Image mask;

    int maximum;
    int current;

    public void SetMax(int m)
    {
        maximum = m;
        current = maximum;
        GetCurrentFill();
    }

    public void SetCurrent(int c)
    {
        current = c;
        GetCurrentFill();
    }

    public void SetPer(float p)
    {
        mask.fillAmount = p;
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        mask.fillAmount = fillAmount;
    }
}

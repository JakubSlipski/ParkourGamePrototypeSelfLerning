using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControll : MonoBehaviour
{
    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void UnPauseTime()
    {
        Time.timeScale = 1f;
    }
    public void SlowMontion(float scale)
    {
        Time.timeScale = scale;
    }


}

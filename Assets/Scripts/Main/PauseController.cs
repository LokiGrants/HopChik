using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : Singleton<PauseController>
{
    public void OnPause()
    {
        Time.timeScale = 0f;
    }

    public void OnUnpause()
    {
        Time.timeScale = 1f;
    }
}

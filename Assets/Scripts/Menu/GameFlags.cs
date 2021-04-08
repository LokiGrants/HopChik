using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlags : Singleton<GameFlags>
{
    [HideInInspector]
    public int deathCounter;

    public void DeadCounter()
    {
        deathCounter++;
        AnalyticsManager.Instance.recordEvent("Death");
    }
}

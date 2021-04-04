using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounter : Singleton<DeathCounter>
{
    [HideInInspector]
    public int deathCounter;

    public void DeadCounter()
    {
        deathCounter++;
    }
}

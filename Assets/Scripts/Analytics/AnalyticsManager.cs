using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
    public void recordEvent(string eventName)
    {
        //level complete
        Analytics.CustomEvent(eventName);
    }
}

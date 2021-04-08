using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnalyticsController : MonoBehaviour
{
    public string eventName;

    public void recordEvent()
    {
        AnalyticsManager.Instance.recordEvent(eventName);
    }

}

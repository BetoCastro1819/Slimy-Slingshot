using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterEventsManager : MonoBehaviour {

    public MeterDetector meterDetector;
    public List<MeterEvent> eventList;

    int eventListIndex = 0;

    private void Update()
    {
        if (eventListIndex < eventList.Count)
        {
            if (meterDetector.GetMeters() >= eventList[eventListIndex].eventAt)
            {
                switch (eventList[eventListIndex].type)
                {
                    case EventType.SPAWN:
                        Instantiate(eventList[eventListIndex].prefabToSPAWN, Camera.main.transform);
                        break;
                    default:
                        break;
                }
                eventListIndex++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    SPAWN,
}

[CreateAssetMenu(fileName = "newMeterEvent", menuName = "MeterEvent")]
public class MeterEvent : ScriptableObject {
    public int eventAt;
    public EventType type;
    public GameObject prefabToSPAWN;
}

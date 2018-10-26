using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    SPAWN,
	ENABLE_OBSTACLES,
	ENABLE_MOVING_ENEMIES,
	ENABLE_SHOOTING_ENEMIES,
}

[CreateAssetMenu(fileName = "newMeterEvent", menuName = "MeterEvent")]
public class MeterEvent : ScriptableObject
{
    public int eventAt;
    public EventType type;
    public GameObject prefabToSPAWN;
}

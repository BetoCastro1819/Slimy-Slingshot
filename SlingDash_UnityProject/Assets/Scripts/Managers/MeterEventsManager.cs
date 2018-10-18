using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterEventsManager : MonoBehaviour
{

    public MeterDetector meterDetector;
    public List<MeterEvent> eventList;

    private GameManager gm;
    private int eventListIndex = 0;

    private void Start()
    {
        gm = GameManager.GetInstance();
    }

    private void Update()
    {

    }
}

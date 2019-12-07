using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetersMission : Mission 
{
    public float metersToComplete = 150;
    float metersTraveled = 0;


    // Use this for initialization
    void Start()
    {
        MeterDetector.OnMetersTraveled += MeterDetector_OnMetersTraveled;
    }

    public override bool IsComplete()
    {
        if (metersTraveled >= metersToComplete)
            return true;

        return false;
    }

    private void MeterDetector_OnMetersTraveled(float _metersTraveled)
    {
        metersTraveled = _metersTraveled;
    }
}

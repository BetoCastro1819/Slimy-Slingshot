using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHitBoss : MonoBehaviour {

    public List<GameObject> criticalPoints;

    [HideInInspector]
    public int criticalPointsQuant;
    bool isdone = false;

    private void Start()
    {
        criticalPointsQuant = criticalPoints.Count;
    }

    private void Update()
    {
        if (criticalPointsQuant <= 0 && !isdone)
        {
            gameObject.layer = LayerMask.NameToLayer("Boss");
            isdone = true;
        }
    }
}

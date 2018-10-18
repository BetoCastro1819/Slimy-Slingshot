using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHitBoss : MonoBehaviour {

    public List<GameObject> criticalPoints;

    [HideInInspector]
    public int criticalPointsQuant;

    private GameManager gm;
    private bool canBeKilled;

    private void Start()
    {
        gm = GameManager.GetInstance();
        criticalPointsQuant = criticalPoints.Count;
        canBeKilled = false;
    }

    private void Update()
    {
        if (criticalPointsQuant <= 0 )
        {
            canBeKilled = true;
            gameObject.layer = LayerMask.NameToLayer("Boss");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {

            if (canBeKilled)
            {
                gm.BossIsActive = false;
                Destroy(gameObject);
            }
        }
    }
}

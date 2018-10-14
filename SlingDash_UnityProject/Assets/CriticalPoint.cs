using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPoint : Enemy {

    public CriticalHitBoss myBoss;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            myBoss.criticalPointsQuant--;
            Destroy(gameObject);
        }    
    }
}

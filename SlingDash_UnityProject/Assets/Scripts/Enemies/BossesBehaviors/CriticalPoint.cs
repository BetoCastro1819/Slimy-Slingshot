using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPoint : Enemy {

    public CriticalHitBoss myBoss;



    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;

            myBoss.criticalPointsQuant--;
            //Destroy(gameObject);
        }    
    }
}

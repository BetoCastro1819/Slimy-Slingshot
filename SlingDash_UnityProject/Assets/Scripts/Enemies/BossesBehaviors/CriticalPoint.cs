using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPoint : Enemy {

    public CriticalHitBoss myBoss;

    private bool isAlive = true;
    private bool isAttacking = false;
    private Vector3 targetPos;
    //private Vector3 startingPos;

    new private void Update()
    {
        if (isAttacking)
        {
            //startingPos = transform.parent.localPosition;
            transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;

            myBoss.criticalPointsQuant--;
            isAlive = false;
            isAttacking = false;
            //Destroy(gameObject);
        }
    }

    public bool IsAlive() {
        return isAlive;
    }

    public void Attack(Vector3 _targetPos)
	{
        targetPos = _targetPos;
        isAttacking = true;
    }
}

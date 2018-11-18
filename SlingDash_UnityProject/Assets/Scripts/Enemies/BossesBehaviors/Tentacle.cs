using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Enemy
{
    public TentacleBoss myBoss;

    private bool isAlive = true;
    private bool isAttacking = false;
    private Vector3 targetPos;

	private Animator animator;
    //private Vector3 startingPos;

	public override void Start()
	{
		base.Start();

		animator = GetComponent<Animator>();
	}

	public void Attack()
	{
		animator.SetTrigger("AttackTrigger");
	}


	/*
    new private void Update()
    {
        if (isAttacking)
        {
            //startingPos = transform.parent.localPosition;
            transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, speed * Time.deltaTime);
        }
    }
	*/
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

    public bool IsAlive()
	{
        return isAlive;
    }
}

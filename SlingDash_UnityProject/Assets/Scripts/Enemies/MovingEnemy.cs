using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    private Vector3 dir = Vector3.right;
	
	// Update is called once per frame
	override public void Update ()
	{
        base.Update();

		if (!killed)
			transform.position += speed * dir * Time.deltaTime;
	}

    override public void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Wall")
            dir *= -1;
    }
}

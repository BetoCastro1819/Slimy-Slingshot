using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    private Vector3 dir = Vector3.right;
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position += speed * dir * Time.deltaTime;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dir *= -1;
    }
}

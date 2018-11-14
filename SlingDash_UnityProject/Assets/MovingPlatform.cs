using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private bool isGoingLeft = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isGoingLeft)
        {
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.velocity = (transform.right * -1) * speed;
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		isGoingLeft = !isGoingLeft;
	}

	public void SetIsGoingLeft(bool _isGoingLeft)
	{
        isGoingLeft = _isGoingLeft;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public float speed = 5f;
	public float offBoundsOffset = 2f;

	private Camera cam;
	protected float offBounds;

	private void Start()
	{
		cam = Camera.main;
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset; 
	}

	void Update()
    {
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;

		if (health <= 0 || transform.position.y < offBounds)
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
		Debug.Log(health);
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "PlayerBullet")
		{
			TakeDamage(1);
			Debug.Log(health);
		}
	}
}

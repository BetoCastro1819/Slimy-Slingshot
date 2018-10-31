using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour {

    public float speed = 5f;
    public float rotateSpeed = 200f;

    private Player player;
    private Rigidbody2D rb;

    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        Vector2 direction = (Vector2)player.transform.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up * -1).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = (transform.up * -1) * speed;
    }

    public void SetPlayer(Player _player) {
        player = _player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 0;
    public float speed = 5f;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}

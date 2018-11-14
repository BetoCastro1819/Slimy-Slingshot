  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHitBoss : MonoBehaviour
{
    public List<CriticalPoint> criticalPoints;
	public float headAttackSpeed = 10f;

    [HideInInspector]
    public int criticalPointsQuant;
    public float timeToAttack = 2f;

	private Animator animator;

    private LevelManager levelManager;
    private PlayerSlimy player;
    private bool canBeKilled;
    private float timer;

    private void Start()
    {
		animator = GetComponent<Animator>();
		levelManager = LevelManager.GetInstance();
        player = FindObjectOfType<PlayerSlimy>();
        criticalPointsQuant = criticalPoints.Count;
        canBeKilled = false;
    }

    private void Update()
    {
		if (criticalPointsQuant > 0)
		{
			timer += Time.deltaTime;
			if (timer >= timeToAttack)
			{
				TentacleAttack();
			}
		}
		else
		{
			HeadAttack();
		}
	}

	void TentacleAttack()
	{
		for (int i = 0; i < criticalPoints.Count; i++)
		{
			if (criticalPoints[i].IsAlive())
			{
				if (player.enabled)
				{
					criticalPoints[i].Attack(player.transform.position);
					timer = 0;
					return;
				}
			}
		}
	}

	void HeadAttack()
	{
		canBeKilled = true;
		gameObject.layer = LayerMask.NameToLayer("Boss");
		animator.enabled = false;

		transform.position = Vector3.Lerp(transform.position, player.transform.position, headAttackSpeed * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            if (canBeKilled)
            {
                levelManager.BossIsActive = false;

                Destroy(gameObject);
            }
        }

		if (collision.gameObject.tag == "Player")
		{
			player.TakeDamage(10);
		}
	}
}

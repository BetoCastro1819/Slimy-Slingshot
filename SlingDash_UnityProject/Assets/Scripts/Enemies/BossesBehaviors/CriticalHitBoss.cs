  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHitBoss : MonoBehaviour
{

    public List<CriticalPoint> criticalPoints;

    [HideInInspector]
    public int criticalPointsQuant;
    public float timeToAttack = 2f;

    private LevelManager levelManager;
    private PlayerSlimy player;
    private bool canBeKilled;
    private float timer;

    private void Start()
    {
        levelManager = LevelManager.GetInstance();
        player = FindObjectOfType<PlayerSlimy>();
        criticalPointsQuant = criticalPoints.Count;
        canBeKilled = false;
    }

    private void Update()
    {
        if (criticalPointsQuant <= 0 )
        {
            canBeKilled = true;
            gameObject.layer = LayerMask.NameToLayer("Boss");
        }

        timer += Time.deltaTime;
        if (timer >= timeToAttack)
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
    }
}

using System;
using UnityEngine;

public class PlayerSlimy : MonoBehaviour
{
	//#region Singleton
	//private static PlayerSlimy instance;
	//public static PlayerSlimy Get()
	//{
	//	return instance;
	//}
//
	//private void Awake()
	//{
	//	instance = this;
	//}
	//#endregion

	public static event Action OnLevelComplete_Event;

	[SerializeField] int health = 1;

	[Header("Aiming State")]
	[SerializeField] GameObject analogStick;
	[SerializeField] GameObject stick;
	[SerializeField] GameObject aimingArrow;
	[SerializeField] float digitalAnalogLimit = 0.5f;
	[SerializeField] float slingshotForce;
	[SerializeField] GameObject playerBulletPrefab;

	[Header("Killed State")]
	[SerializeField] GameObject playerDeathEffect;

	[Header("Respawn State")]
	[SerializeField] float timeToRespawn;

	public Rigidbody2D PlayerRigidbody { get; set; }

	public StateMoving StateMoving { get; set; }
	public StateAiming StateAiming { get; set; }
	public StateKilled StateKilled { get; set; }

	private Vector2 mousePos;
	private Rigidbody2D playerRigidbody;
	private Animator animator;

	private enum PlayerStateEnum
	{
		Idle,
		Aiming,
		Respawning
	}
	private PlayerStateEnum stateEnum;

	void Start ()
	{
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;

		animator = GetComponent<Animator>();

		stateEnum = PlayerStateEnum.Idle;
	}

	void Update ()
	{
		if (LevelBased.LevelManager.Instance.state != LevelBased.LevelManager.GameState.OnPause)
		{
			UpdateState();
		}
	}

	void UpdateState()
	{
		switch(stateEnum)
		{
			case PlayerStateEnum.Idle: Idle(); 
				break;
			case PlayerStateEnum.Aiming: Aiming(); 
				break;
			case PlayerStateEnum.Respawning: Respawning(); 
				break;
		}
	}

	void Idle()
	{
		animator.SetBool("OnAiming", false);

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			stateEnum = PlayerStateEnum.Aiming;
			OnAimingEnter();
		}
	}

	void OnAimingEnter()
	{
		animator.SetBool("OnAiming", true);

		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		analogStick.transform.position = new Vector2(mousePos.x, mousePos.y);
		analogStick.SetActive(true);

		aimingArrow.SetActive(true);
	}

	void Aiming()
	{
		TimeScaleManager.Instance.EnableSlowMoTimeScale();

		HandleAimingDirection();

		if(Input.GetKeyUp(KeyCode.Mouse0))
			OnAimingExit();
	}

	void HandleAimingDirection()
	{
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		Vector2 shootingDir = mousePos - (Vector2)analogStick.transform.position;

		if (shootingDir.x == 0 && shootingDir.y == 0)
			shootingDir = -transform.up;

		stick.transform.position = mousePos;
		if (Vector2.Distance(analogStick.transform.position, mousePos) > digitalAnalogLimit)
			stick.transform.localPosition = new Vector2(0, -digitalAnalogLimit);

		analogStick.transform.up = -shootingDir;
		transform.up = -shootingDir;
	}

	void OnAimingExit()
	{
		stateEnum = PlayerStateEnum.Idle;

		TimeScaleManager.Instance.DisableSlowMoTimeScale();

		analogStick.SetActive(false);
		aimingArrow.SetActive(false);

		Shoot();
	}

	void Shoot()
	{
		GameObject bullet = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
		bullet.transform.up = -transform.up;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.AddForce(transform.up * slingshotForce);
	}

	void Respawning()
	{

	}

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.CompareTag("Portal"))
		{
			OnLevelComplete_Event();
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();

		if (enemy != null)
			Kill();
	}

	public void Kill()
	{

	}

}

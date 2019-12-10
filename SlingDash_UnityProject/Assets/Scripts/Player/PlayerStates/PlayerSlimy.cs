using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSlimy : MonoBehaviour
{
	public static event Action OnLevelComplete_Event;

	[Header("Trail particles")]
	[SerializeField] ParticleSystem trailParticleSystem;

	[Header("Audio")]
	[SerializeField] AudioClip shotSound;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip onLevelCompleteSound;

	//[SerializeField] int health = 1;

	[Header("Aiming State")]
	[SerializeField] GameObject analogStick;
	[SerializeField] GameObject stick;
	[SerializeField] GameObject aimingArrow;
	[SerializeField] float digitalAnalogLimit = 0.5f;
	[SerializeField] float slingshotForce;
	[SerializeField] GameObject playerBulletPrefab;

	[Header("Killed State")]
	[SerializeField] float forceToApplyOnKilled;
	[SerializeField] float lowScreenBoundToRespawn;

	[Header("Respawn State")]
	[SerializeField] GameObject playerRespawnEffect;
	[SerializeField] float timeToRespawn;

	public Rigidbody2D PlayerRigidbody { get; set; }

	public StateMoving StateMoving { get; set; }
	public StateAiming StateAiming { get; set; }
	public StateKilled StateKilled { get; set; }


	public event Action<int> OnSlingshotCounterIncreased_Event;
	public event Action OnLiveLost_Event;
	private int slingshotCounter;

	private enum PlayerStateEnum
	{
		Idle,
		Aiming,
		OnKilled
	}
	private PlayerStateEnum stateEnum;

	private Vector2 mousePos;
	private Rigidbody2D playerRigidbody;
	private Animator animator;
	private Vector2 spawnPosition;
	private CameraShake cameraShake;
	private bool gameOver;


	void Start ()
	{
		Renderer particleRenderer = trailParticleSystem.GetComponent<Renderer>();
		string currentPlayerTrail = PersistentGameData.Instance.gameData.currentPlayerTrail;
		particleRenderer.material = Resources.Load<Material>(currentPlayerTrail);
		
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;

		animator = GetComponent<Animator>();

		stateEnum = PlayerStateEnum.Idle;

		slingshotCounter = 0;

		spawnPosition = transform.position;

		cameraShake = Camera.main.GetComponent<CameraShake>();

		gameOver = false;
	}

	void Update ()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

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
			case PlayerStateEnum.OnKilled: OnKilled();
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
		AudioManager.Instance.PlayAudioClip(shotSound);

		GameObject bullet = Instantiate(playerBulletPrefab, transform.position + (-transform.up), transform.rotation);
		bullet.transform.up = -transform.up;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.AddForce(transform.up * slingshotForce);

		slingshotCounter++;
		OnSlingshotCounterIncreased_Event(slingshotCounter);
	}

	void OnKilled()
	{
		transform.up = playerRigidbody.velocity;


		if (transform.position.y < lowScreenBoundToRespawn)
		{
			if (!gameOver)
				Respawn();
			else
				Time.timeScale = 0;
		}
	}

	void Respawn()
	{
		transform.position = spawnPosition;

		if (playerRigidbody != null)
		{
			playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			playerRigidbody.velocity = Vector2.zero;
		}

		Collider2D collider = GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = true;
		}

		transform.up = Vector2.up;

		stateEnum = PlayerStateEnum.Idle;
		animator.SetBool("OnKilled", false);

		Instantiate(playerRespawnEffect, transform.position, Quaternion.identity);
	}

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.CompareTag("Portal"))
		{
			AudioManager.Instance.PlayAudioClip(onLevelCompleteSound);
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
		AudioManager.Instance.PlayAudioClip(deathSound);

		analogStick.SetActive(false);
		aimingArrow.SetActive(false);

		animator.SetBool("OnKilled", true);

		cameraShake.StartShake();

		transform.up = -Vector2.up;

		if (playerRigidbody != null)
		{
			playerRigidbody.constraints = RigidbodyConstraints2D.None;

			playerRigidbody.velocity = Vector2.zero;
			playerRigidbody.AddForce(Vector2.up * forceToApplyOnKilled);
		}

		Collider2D collider = GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = false;
		}

		stateEnum = PlayerStateEnum.OnKilled;

		Time.timeScale = 1;

		OnLiveLost_Event();
	}

	public void OnGameOver()
	{
		gameOver = true;
	}
}

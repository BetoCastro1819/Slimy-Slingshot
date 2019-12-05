using UnityEngine;

public class PlayerSlimy : MonoBehaviour
{
	#region Singleton
	private static PlayerSlimy instance;
	public static PlayerSlimy Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}
	#endregion

	public int health = 1;

	[Header("Aiming State")]
	public Sprite playerAiming;
	private SpriteRenderer tailSpriteRenderer;
	public GameObject tail;
	public Sprite tailStreched;
	public Sprite tailDefault;
	public GameObject analogStick;
	public GameObject stick;
	public GameObject aimingArrow;

	public Rigidbody2D PlayerRigidbody { get; set; }

	public StateMoving StateMoving { get; set; }
	public StateAiming StateAiming { get; set; }
	public StateKilled StateKilled { get; set; }

	private Vector3 mousePos;
	private SpriteRenderer spriteRenderer;

	private enum PlayerStateEnum
	{
		Idle,
		Aiming,
		Killed,
		Respawn
	}
	private PlayerStateEnum stateEnum;

	void Start ()
	{
		PlayerRigidbody = GetComponent<Rigidbody2D>();
		PlayerRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;

		spriteRenderer = GetComponent<SpriteRenderer>();
		tailSpriteRenderer = tail.GetComponent<SpriteRenderer>();

		stateEnum = PlayerStateEnum.Idle;
	}

	void Update ()
	{
		UpdateState();
	}

	void UpdateState()
	{
		switch(stateEnum)
		{
			case PlayerStateEnum.Idle: Idle(); 
				break;
			case PlayerStateEnum.Aiming: Aiming(); 
				break;
			case PlayerStateEnum.Killed: Killed(); 
				break;
			case PlayerStateEnum.Respawn: Respawn(); 
				break;
		}
	}

	void Idle()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			stateEnum = PlayerStateEnum.Aiming;
			OnScreenTapped();
		}
	}

	void OnScreenTapped()
	{
		PlayerRigidbody.interpolation = RigidbodyInterpolation2D.None;

		SetSprite(playerAiming);
		tailSpriteRenderer.sprite = tailStreched;

		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		analogStick.transform.position = new Vector2(mousePos.x, mousePos.y);
		analogStick.SetActive(true);

		aimingArrow.SetActive(true);
	}

	void Aiming()
	{

	}

	void Killed()
	{

	}

	void Respawn()
	{

	}

	void KillPlayer()
	{
		StateKilled.Enter();
		SetState(StateKilled);
	}

	public void SetState(PlayerState state)
	{
	}

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();

		if (enemy != null)
		{
			KillPlayer();
		}
	}
}

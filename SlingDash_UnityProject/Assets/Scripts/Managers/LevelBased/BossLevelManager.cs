using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossLevelManager : MonoBehaviour 
{
	[Header("Player variables")]
	[SerializeField] PlayerSlimy player;
	[SerializeField] int playersMaxLives;

	[Header("Level Elements")]
	[SerializeField] GameObject boss;
	[SerializeField] GameObject portal;

	[Header("UI")]
	[SerializeField] GameObject gameOverScreen;
	[SerializeField] Text playerLivesLeftText;

	private int playersLivesLeft;
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();

		playersLivesLeft = playersMaxLives;
		UpdatePlayerLivesUI();

		player.OnLiveLost_Event += PlayerWasKilled;
	}

	private void PlayerWasKilled()
	{
		playersLivesLeft--;

		if (playersLivesLeft < 0)
		{
			gameOverScreen.SetActive(true);
			player.OnGameOver();
		}
		else 
		{
			animator.SetBool("PlayerLostLive", true);		
		}
	}

	private void Update()
	{
		if (boss == null)
			portal.SetActive(true);
	}

	public void EnableBoss()
	{
		boss.SetActive(true);
	}

	public void UpdatePlayerLivesUI()
	{
		playerLivesLeftText.text = "x " + playersLivesLeft;
		animator.SetBool("PlayerLostLive", false);		
	}

	public void OpenScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void Replay()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

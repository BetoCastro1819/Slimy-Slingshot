using UnityEngine;
using UnityEngine.UI;

public class BossLevelManager : MonoBehaviour 
{
	[SerializeField] GameObject boss;
	[SerializeField] GameObject portal;
	[SerializeField] Text playerLivesLeftText;

	private void Update()
	{
		if (boss == null)
			portal.SetActive(true);
	}

	public void EnableBoss()
	{
		boss.SetActive(true);
	}
}

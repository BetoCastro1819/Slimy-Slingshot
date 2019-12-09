using UnityEngine;
using UnityEngine.UI;

public class BossLevelManager : MonoBehaviour 
{
	[SerializeField] GameObject boss;
	[SerializeField] Text playerLivesLeftText;

	public void EnableBoss()
	{
		boss.SetActive(true);
	}
}

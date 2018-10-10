using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public GameObject tapAndHold;
	public GameObject dragDown;
	public GameObject moveToAim;
	public GameObject releaseToShoot;
	public GameObject getToFinishLine;
	public GameObject tutorialCircle;

	public float timeToChangeText = 2f;

	private float timer = 0;
	private string tutorialKey = "TutorialPlayed";

	#region Singleton
	private static TutorialManager instance;
	public static TutorialManager Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}
	#endregion
	void Start ()
	{
		PlayerPrefs.SetInt(tutorialKey, 0); // 0 = false

		tapAndHold.SetActive(true);
		tutorialCircle.SetActive(true);
		dragDown.SetActive(false);
		moveToAim.SetActive(false);
		releaseToShoot.SetActive(false);
		getToFinishLine.SetActive(false);
		timer = 0;
	}

	void Update ()
	{
		if (tapAndHold.activeSelf == true)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				tapAndHold.SetActive(false);
				tutorialCircle.SetActive(false);
				dragDown.SetActive(true);
			}
		}

		if (dragDown.activeSelf == true)
		{
			timer += Time.unscaledDeltaTime;
			if (timer > timeToChangeText)
			{
				timer = 0;
				dragDown.SetActive(false);
				moveToAim.SetActive(true);
			}
		}

		if (moveToAim.activeSelf == true)
		{
			timer += Time.unscaledDeltaTime;
			if (timer > timeToChangeText)
			{
				timer = 0;
				moveToAim.SetActive(false);
				releaseToShoot.SetActive(true);
			}
		}

		if (releaseToShoot.activeSelf == true)
		{
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				releaseToShoot.SetActive(false);
				getToFinishLine.SetActive(true);
			}
		}
	}

	public bool GetTutorialPlayed()
	{
		return PlayerPrefs.GetInt(tutorialKey) == 1;
	}

	public void SetTutorialPlayed(bool tutorialCompleted)
	{
		if (tutorialCompleted)
			PlayerPrefs.SetInt(tutorialKey, 1);
	}
}

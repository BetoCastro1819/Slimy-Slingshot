using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
	public GameObject clearScreenTrigger;
    public GameObject playerPrefab;

    float ReviveY;
    MeterDetector pmd;

    private void Start()
    {
        pmd = FindObjectOfType<MeterDetector>();
    }

    private void Update()
    {
        ReviveY = pmd.GetMetersTravelled();
    }

    public void RevivePlayerAtPoint(Vector3 revivePos)
    {

        // Creates a trigger the bigger than the screen
        // Destroys every object that has the "Erasable" component
		Instantiate(clearScreenTrigger, revivePos, Quaternion.identity);

        PlayerSlimy player = GameManager.GetInstance().player;
        player.gameObject.SetActive(true);
        player.transform.SetPositionAndRotation(revivePos, Quaternion.identity);
		player.SetState(player.StateMoving);
        player.health = 1;

		GameManager.GetInstance().SetState(GameManager.GameState.ON_START);
	}

	public float GetReviveY()
    {
        return ReviveY;
    }
}

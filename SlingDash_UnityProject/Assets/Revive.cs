using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour {

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
        ReviveY = pmd.GetMaxMeters();
    }

    public void RevivePlayerAtPoint(Vector3 revivePos) {
        GameManager.GetInstance().SetState(GameManager.GameState.PLAYING);

		GameObject clearScreen = Instantiate(clearScreenTrigger, revivePos, Quaternion.identity);
		//Destroy(clearScreen, 2f);

        Player player = GameManager.GetInstance().player;
        player.gameObject.SetActive(true);
        player.transform.SetPositionAndRotation(revivePos, Quaternion.identity);
        player.playerState = Player.PlayerState.MOVING;
        player.health = 1;
    }

    public float GetReviveY() {
        return ReviveY;
    }
}

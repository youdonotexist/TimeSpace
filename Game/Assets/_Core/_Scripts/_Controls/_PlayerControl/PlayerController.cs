using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<Vector2, GameObject>("PlayerMove", PlayerMove);
	}
	
	void PlayerMove(Vector2 move, GameObject player) {
		Vector3 pos = player.transform.position;
		pos.x += move.x * 0.1f;
		pos.y += move.y * 0.1f;
		player.transform.position = pos;

		if (move != Vector2.zero) {
			player.transform.up = move.normalized * -1.0f;
		}

	}
}

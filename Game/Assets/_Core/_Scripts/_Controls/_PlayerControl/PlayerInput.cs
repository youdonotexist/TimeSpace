using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	// Use this for initialization
	void Update () {
		Vector2 direction = Vector2.zero;
		if (Input.GetKey(KeyCode.UpArrow)) {
			direction.y = 1.0f;
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			direction.y = -1.0f;
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			direction.x = 1.0f;
		}
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			direction.x = -1.0f;
		}

		Messenger.Broadcast("PlayerMove", direction, gameObject);
	}
}

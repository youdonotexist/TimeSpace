using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TSClientController : Singleton<TSClientController>
{
	Ship _ownedShip;
	Player _ownedPlayer;

	Dictionary<string, string> clientStorage = new Dictionary<string, string>();


	public void SetOwnedShip(Ship ship) {
		_ownedShip = ship;
	}

	public void SetOwnedPlayer(Player player) {
		_ownedPlayer = player;
		_ownedPlayer.transform.parent = _ownedShip.transform;
		_ownedPlayer.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
	}

	void Update() {
		if (_ownedShip) {
			UpdateShip();
		}

		if (_ownedPlayer) {
			UpdatePlayer();
		}

	}
	void UpdateShip() {
		float hInput = Input.GetAxis("Horizontal");
		float vInput = Input.GetAxis("Vertical");
		if (hInput != 0 || vInput != 0)
		{
			_ownedShip.networkView.RPC("SetVelocity", uLink.NetworkPlayer.server, hInput, vInput);
		}
	}

	void UpdatePlayer() {
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

		_ownedPlayer.networkView.RPC("MovePlayer", uLink.NetworkPlayer.server, direction);
	}

	public void PutStorageValue(string key, string value) {
		clientStorage.Add(key, value);
	}

	public string GetStorageValue(string key) {
		string value = null;
		clientStorage.TryGetValue(key, out value);

		return value;
	}
}


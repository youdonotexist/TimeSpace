using UnityEngine;
using System.Collections;

public class Player : uLink.MonoBehaviour
{
	void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info) {
		string storedPlayerId = TSClientController.Instance.GetStorageValue("playerId");
		string incomingPlayerId = info.networkView.initialData.Read<string>();
		if (incomingPlayerId == storedPlayerId) {
			TSClientController.Instance.SetOwnedPlayer(this);
		}
	}
	
	[RPC]
	void MovePlayer(Vector2 move)
	{
		//rigidbody.MovePosition(new Vector3(rigidbody.position.x + move.x, rigidbody.position.y + move.y, rigidbody.position.z));
		//rigidbody.AddForce(new Vector2(move.x * 2.0f, move.y * 2.0f));
		//Vector3 pos = transform.position;
		//pos.x += move.x * 2.0f * Time.deltaTime;
		//pos.y += move.y * 2.0f * Time.deltaTime;
		//transform.position = pos;
		transform.position += new Vector3(move.x * Time.deltaTime * 2.0f, move.y * Time.deltaTime * 2.0f, 0.0f);
		if (move != Vector2.zero) {
			transform.up = -move.normalized;
		}
		//GetComponent<CharacterController2D>().move(move * 2.0f);
	}
	
}


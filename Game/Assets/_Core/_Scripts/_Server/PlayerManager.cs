using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
	Dictionary<string, GameObject> playerMapper = new Dictionary<string, GameObject>();

	public void BindPlayer(string remoteKey, GameObject networkKey) {
		playerMapper.Add (remoteKey, networkKey);
	}
	
	public GameObject GetBoundPlayer(string remoteKey) {
		GameObject value = null;
		playerMapper.TryGetValue(remoteKey, out value);
		return value;
	}
}


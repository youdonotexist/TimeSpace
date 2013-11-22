using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipManager : Singleton<ShipManager>
{
	Dictionary<string, GameObject> shipMapper = new Dictionary<string, GameObject>();

	public void BindShip(string remoteKey, GameObject networkKey) {
		shipMapper.Add (remoteKey, networkKey);
	}

	public GameObject GetBoundShip(string remoteKey) {
		GameObject value = null;
		shipMapper.TryGetValue(remoteKey, out value);
		return value;
	}
}


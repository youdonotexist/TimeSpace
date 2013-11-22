using UnityEngine;
using System.Collections;

public class Ship : uLink.MonoBehaviour
{
	public GameObject[] cellPrefabs;

	void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info) {
		string shipId = info.networkView.initialData.Read<string>();
		string cellsJson = info.networkView.initialData.Read<string>();
		IDictionary ship = (IDictionary) MiniJSON.Json.Deserialize(cellsJson);
		IList cells = (IList) ship["cells"]; 
		
		for (int i = 0; i < cells.Count; i++) {
			IDictionary cell = (IDictionary) cells[i];
			object o = cell["type"];
			long type = (long) cell["type"];
			GameObject prefab = cellPrefabs[type];
			GameObject go = (GameObject) GameObject.Instantiate(prefab, new Vector3((long) cell["X"], (long) cell["Y"], 0.0f), prefab.transform.rotation);
			go.GetComponent<tk2dSprite>().SetSize(new Vector2(20.0f, 20.0f));
			go.transform.parent = transform;
		}
		
		//GameObject playa = (GameObject) GameObject.Instantiate(playerPrefab, shipInstance.transform.position, playerPrefab.transform.rotation);
		//playa.transform.parent = shipInstance.transform;
		//playa.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);

		//TODO: Identify the player-owner of this ship to find out if we need to set these values
		string controlledShipid = TSClientController.Instance.GetStorageValue("shipId");
		if (shipId == controlledShipid) {
			Camera.main.GetComponent<FollowTrackingCamera>().target = transform;
			TSClientController.Instance.SetOwnedShip(this);
		}
	}

	[RPC]
	void SetVelocity(float horizontalAmount, float verticalAmount)
	{
		rigidbody.AddForce(new Vector3(horizontalAmount * 5000.0f, verticalAmount * 5000.0f, 0.0f));
	}


}


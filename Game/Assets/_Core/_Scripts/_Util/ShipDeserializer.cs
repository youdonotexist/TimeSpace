using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class ShipDeserializer : MonoBehaviour {
	public GameObject[] cellPrefabs;
	public GameObject playerPrefab;
	public GameObject shipPrefab;
	string rawJson = "{\"cells\":[{\"X\":2,\"Y\":-8,\"type\":1},{\"X\":3,\"Y\":-8,\"type\":1},{\"X\":4,\"Y\":-8,\"type\":1},{\"X\":-1,\"Y\":-7,\"type\":1},{\"X\":0,\"Y\":-7,\"type\":1},{\"X\":1,\"Y\":-7,\"type\":1},{\"X\":2,\"Y\":-7,\"type\":2},{\"X\":3,\"Y\":-7,\"type\":2},{\"X\":4,\"Y\":-7,\"type\":2},{\"X\":5,\"Y\":-7,\"type\":1},{\"X\":6,\"Y\":-7,\"type\":1},{\"X\":-2,\"Y\":-6,\"type\":1},{\"X\":-1,\"Y\":-6,\"type\":2},{\"X\":0,\"Y\":-6,\"type\":2},{\"X\":1,\"Y\":-6,\"type\":2},{\"X\":2,\"Y\":-6,\"type\":2},{\"X\":3,\"Y\":-6,\"type\":2},{\"X\":4,\"Y\":-6,\"type\":2},{\"X\":5,\"Y\":-6,\"type\":2},{\"X\":6,\"Y\":-6,\"type\":2},{\"X\":7,\"Y\":-6,\"type\":1},{\"X\":-3,\"Y\":-5,\"type\":1},{\"X\":-2,\"Y\":-5,\"type\":2},{\"X\":-1,\"Y\":-5,\"type\":2},{\"X\":0,\"Y\":-5,\"type\":2},{\"X\":1,\"Y\":-5,\"type\":2},{\"X\":2,\"Y\":-5,\"type\":2},{\"X\":3,\"Y\":-5,\"type\":2},{\"X\":4,\"Y\":-5,\"type\":2},{\"X\":5,\"Y\":-5,\"type\":2},{\"X\":6,\"Y\":-5,\"type\":2},{\"X\":7,\"Y\":-5,\"type\":2},{\"X\":8,\"Y\":-5,\"type\":1},{\"X\":-3,\"Y\":-4,\"type\":1},{\"X\":-2,\"Y\":-4,\"type\":2},{\"X\":-1,\"Y\":-4,\"type\":2},{\"X\":0,\"Y\":-4,\"type\":2},{\"X\":1,\"Y\":-4,\"type\":2},{\"X\":2,\"Y\":-4,\"type\":2},{\"X\":3,\"Y\":-4,\"type\":2},{\"X\":4,\"Y\":-4,\"type\":2},{\"X\":5,\"Y\":-4,\"type\":2},{\"X\":6,\"Y\":-4,\"type\":2},{\"X\":7,\"Y\":-4,\"type\":2},{\"X\":8,\"Y\":-4,\"type\":1},{\"X\":-3,\"Y\":-3,\"type\":1},{\"X\":-2,\"Y\":-3,\"type\":2},{\"X\":-1,\"Y\":-3,\"type\":2},{\"X\":0,\"Y\":-3,\"type\":2},{\"X\":1,\"Y\":-3,\"type\":2},{\"X\":2,\"Y\":-3,\"type\":2},{\"X\":3,\"Y\":-3,\"type\":2},{\"X\":4,\"Y\":-3,\"type\":2},{\"X\":5,\"Y\":-3,\"type\":2},{\"X\":6,\"Y\":-3,\"type\":2},{\"X\":7,\"Y\":-3,\"type\":2},{\"X\":8,\"Y\":-3,\"type\":1},{\"X\":-3,\"Y\":-2,\"type\":1},{\"X\":-2,\"Y\":-2,\"type\":2},{\"X\":-1,\"Y\":-2,\"type\":2},{\"X\":0,\"Y\":-2,\"type\":2},{\"X\":1,\"Y\":-2,\"type\":2},{\"X\":2,\"Y\":-2,\"type\":2},{\"X\":3,\"Y\":-2,\"type\":2},{\"X\":4,\"Y\":-2,\"type\":2},{\"X\":5,\"Y\":-2,\"type\":2},{\"X\":6,\"Y\":-2,\"type\":2},{\"X\":7,\"Y\":-2,\"type\":2},{\"X\":8,\"Y\":-2,\"type\":2},{\"X\":9,\"Y\":-2,\"type\":1},{\"X\":-3,\"Y\":-1,\"type\":1},{\"X\":-2,\"Y\":-1,\"type\":2},{\"X\":-1,\"Y\":-1,\"type\":2},{\"X\":0,\"Y\":-1,\"type\":2},{\"X\":1,\"Y\":-1,\"type\":2},{\"X\":2,\"Y\":-1,\"type\":2},{\"X\":3,\"Y\":-1,\"type\":2},{\"X\":4,\"Y\":-1,\"type\":2},{\"X\":5,\"Y\":-1,\"type\":2},{\"X\":6,\"Y\":-1,\"type\":2},{\"X\":7,\"Y\":-1,\"type\":2},{\"X\":8,\"Y\":-1,\"type\":2},{\"X\":9,\"Y\":-1,\"type\":1},{\"X\":-3,\"Y\":0,\"type\":1},{\"X\":-2,\"Y\":0,\"type\":2},{\"X\":-1,\"Y\":0,\"type\":2},{\"X\":0,\"Y\":0,\"type\":2},{\"X\":1,\"Y\":0,\"type\":2},{\"X\":2,\"Y\":0,\"type\":2},{\"X\":3,\"Y\":0,\"type\":2},{\"X\":4,\"Y\":0,\"type\":2},{\"X\":5,\"Y\":0,\"type\":2},{\"X\":6,\"Y\":0,\"type\":2},{\"X\":7,\"Y\":0,\"type\":2},{\"X\":8,\"Y\":0,\"type\":2},{\"X\":9,\"Y\":0,\"type\":1},{\"X\":-3,\"Y\":1,\"type\":1},{\"X\":-2,\"Y\":1,\"type\":2},{\"X\":-1,\"Y\":1,\"type\":2},{\"X\":0,\"Y\":1,\"type\":2},{\"X\":1,\"Y\":1,\"type\":2},{\"X\":2,\"Y\":1,\"type\":2},{\"X\":3,\"Y\":1,\"type\":2},{\"X\":4,\"Y\":1,\"type\":2},{\"X\":5,\"Y\":1,\"type\":2},{\"X\":6,\"Y\":1,\"type\":2},{\"X\":7,\"Y\":1,\"type\":2},{\"X\":8,\"Y\":1,\"type\":1},{\"X\":-3,\"Y\":2,\"type\":1},{\"X\":-2,\"Y\":2,\"type\":2},{\"X\":-1,\"Y\":2,\"type\":2},{\"X\":0,\"Y\":2,\"type\":2},{\"X\":1,\"Y\":2,\"type\":2},{\"X\":2,\"Y\":2,\"type\":2},{\"X\":3,\"Y\":2,\"type\":2},{\"X\":4,\"Y\":2,\"type\":2},{\"X\":5,\"Y\":2,\"type\":2},{\"X\":6,\"Y\":2,\"type\":2},{\"X\":7,\"Y\":2,\"type\":1},{\"X\":-3,\"Y\":3,\"type\":1},{\"X\":-2,\"Y\":3,\"type\":2},{\"X\":-1,\"Y\":3,\"type\":2},{\"X\":0,\"Y\":3,\"type\":2},{\"X\":1,\"Y\":3,\"type\":2},{\"X\":2,\"Y\":3,\"type\":2},{\"X\":3,\"Y\":3,\"type\":1},{\"X\":4,\"Y\":3,\"type\":1},{\"X\":5,\"Y\":3,\"type\":1},{\"X\":6,\"Y\":3,\"type\":1},{\"X\":-2,\"Y\":4,\"type\":1},{\"X\":-1,\"Y\":4,\"type\":1},{\"X\":0,\"Y\":4,\"type\":1},{\"X\":1,\"Y\":4,\"type\":1},{\"X\":2,\"Y\":4,\"type\":1}]}";
	IList remoteShips;

	void Awake() {
		Messenger.AddListener<string>("shipSelectEvent", OnShipSelect);
	}

	// Use this for initialization
	void Start () {
		new CI.Request().GET("http://spacetime.aws.af.cm/ship", false,  (request) => {
			GameObject go = GameObject.Find("Popup List");
			UIPopupList menu = go.GetComponent<UIPopupList>();
			remoteShips = (IList) Json.Deserialize(request.ResponseText);
			List<string> names = new List<string>();
			for (int i = 0; i < remoteShips.Count; i++) {
				IDictionary ship = (IDictionary) remoteShips[i];
				names.Add ((string) ship["_id"]);
			}
			menu.items = names;
		}) ;
	}
	
	void OnShipSelect(string selectedString) {
		if (remoteShips != null) {
			for (int j = 0; j < remoteShips.Count; j++) {
				IDictionary ship = (IDictionary) remoteShips[j];
				if (((string) ship["_id"]).Equals(selectedString)) {
					IList cells = (IList) ship["cells"];

					GameObject shipInstance = (GameObject) GameObject.Instantiate(shipPrefab, Vector3.zero, shipPrefab.transform.rotation);
					
					for (int i = 0; i < cells.Count; i++) {
						IDictionary cell = (IDictionary) cells[i];
						object o = cell["type"];
						long type = (long) cell["type"];
						GameObject prefab = cellPrefabs[type];
						GameObject go = (GameObject) GameObject.Instantiate(prefab, new Vector3((long) cell["X"], (long) cell["Y"], 0.0f), prefab.transform.rotation);
						go.GetComponent<tk2dSprite>().SetSize(new Vector2(20.0f, 20.0f));
						go.transform.parent = shipInstance.transform;
					}

					GameObject playa = (GameObject) GameObject.Instantiate(playerPrefab, shipInstance.transform.position, playerPrefab.transform.rotation);
					playa.transform.parent = shipInstance.transform;
					playa.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);

					Camera.main.GetComponent<FollowTrackingCamera>().target = shipInstance.transform;

					break;
				}
			}
		}
	}
}

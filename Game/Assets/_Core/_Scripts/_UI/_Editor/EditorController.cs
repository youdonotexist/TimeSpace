using UnityEngine;
using System.Collections;

public class EditorController : MonoBehaviour
{
	void Start() {

	}

	void OnClick() {
		GameObject current = UICamera.currentTouch.current;
		string buttonName = "";
		if (current != null) {
			buttonName = current.name;
		}
		else {
			return;
		}
		
		if (buttonName == "ExteriorWalls") {
			Messenger.Broadcast("exteriorWallsEvent");
		}
		else if (buttonName == "InteriorWalls") {
			Messenger.Broadcast("interiorWallsEvent");
		}
		else if (buttonName == "Thrusters") {
			Messenger.Broadcast("thrustersEvent");
		}
		else if (buttonName == "Out") {
			Messenger.Broadcast("outEvent");
		}
	}
}


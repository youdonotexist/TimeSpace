using UnityEngine;
using System.Collections;

public class ShipSelectController : MonoBehaviour {

	void Start() {
		GetComponent<UIPopupList>().onSelectionChange = OnSelectionChange;
	}

	void OnSelectionChange (string val)
	{
		Messenger.Broadcast("shipSelectEvent", UIPopupList.current.value);
	}
}

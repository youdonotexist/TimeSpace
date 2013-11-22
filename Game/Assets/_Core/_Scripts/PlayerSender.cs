using UnityEngine;
using System.Collections;

public class PlayerSender : uLink.MonoBehaviour {
	float arrowKeysVelocity = 30.0f;
	
	void Start() {
		if (networkView.isOwner) {
			Camera.main.GetComponent<CameraTracking>().target = transform;	
		}
	}
	
	void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        if (hInput != 0 || vInput != 0)
        {
            networkView.RPC("SetVelocity", uLink.NetworkPlayer.server, hInput * arrowKeysVelocity, vInput * arrowKeysVelocity);
        }
    }
}

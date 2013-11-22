using UnityEngine;
using System.Collections;

public class PlayerReceiver : uLink.MonoBehaviour
{

	[RPC]
    void SetVelocity(float horizontalAmount, float verticalAmount)
    {
        this.rigidbody.velocity = new Vector3(horizontalAmount, verticalAmount, 0);
    }
}


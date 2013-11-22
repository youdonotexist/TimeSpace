using UnityEngine;
using System.Collections;

public class uLinkSmoothPosition : uLink.MonoBehaviour {
	private class State
	{
		public double timestamp = 0;
		public Vector3 pos;
		public Vector3 rot;
	}

	public double interpolationBackTimeMs = 200;
	
	private State mostCurrentReceivedState = new State();

	void uLink_OnSerializeNetworkViewOwner(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		// This callback implementation is needed for getting statesync to the 
		// owner prefab (from the creator prefab on server)
		uLink_OnSerializeNetworkView(stream, info);
	}
	
	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			// Send information to all proxies and the owner (clients)
			// This code is executed on the creator (server) 
			stream.Write(transform.localPosition);
			stream.Write(transform.up); 
			
		}
		else
		{
			//This code is executed in the clients
			mostCurrentReceivedState.pos = stream.Read<Vector3>();
			mostCurrentReceivedState.rot = stream.Read<Vector3>(); 
			
			mostCurrentReceivedState.timestamp = info.timestamp + (interpolationBackTimeMs / 1000);
		}
	}

	void FixedUpdate()
	{
		if (networkView.viewID == uLink.NetworkViewID.unassigned)
		{
			return;
		}
		
		// Only execute the smooth rotation for proxies and owner, not authority
		if (networkView.hasAuthority)
		{
			return;
		}
		
		//No state sync message has arrived to this client => Do nothing.
		if (mostCurrentReceivedState.timestamp == 0)
		{
			return;
		}
		
		transform.localPosition = mostCurrentReceivedState.pos;
		transform.up = mostCurrentReceivedState.rot;
	}
}

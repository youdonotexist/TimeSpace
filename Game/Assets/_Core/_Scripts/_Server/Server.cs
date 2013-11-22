using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Server : uLink.MonoBehaviour
{
	public static string API_URL = "http://spacetime.aws.af.cm/";
	//public static string API_URL = "http://localhost:3000/";

	[Serializable]
	public class InstantiateOnConnected
	{
		public Vector3 startPosition = new Vector3(0, 3, 0);
		public Vector3 startRotation = new Vector3(0, 0, 0);

		public GameObject oPlayerPrefab;
		public GameObject pPlayerPrefab;
		public GameObject sPlayerPrefab;

		public GameObject oShipPrefab;
		public GameObject pShipPrefab;
		public GameObject sShipPrefab;

		public bool appendLoginData = false;

		GameObject GetShip(string shipId) {
			if (shipId != null && shipId.Length > 0) {
				return ShipManager.Instance.GetBoundShip(shipId);
			}
			
			return null;
		}

		public void Instantiate(uLink.NetworkPlayer player)
		{
			//if (oPlayerPrefab != null && pPlayerPrefab != null && sPlayerPrefab != null)
			{
				Quaternion rotation = Quaternion.Euler(startRotation);
				string playerId = player.loginData.Read<string>();
				string shipId = player.loginData.Read<string>();

				if (PlayerManager.Instance.GetBoundPlayer(playerId) == null) { //Player hasn't connected yet
					GameObject instantiatedShip = GetShip(shipId);
					if (instantiatedShip == null) { //Ship isn't in play, yet
						new CI.Request().Send (API_URL + "ship/" + shipId, "GET", false, (request) => {
							instantiatedShip = uLink.Network.Instantiate(uLink.NetworkPlayer.server, oShipPrefab, pShipPrefab, sShipPrefab, Vector3.zero, Quaternion.identity, 0, shipId, request.ResponseText);	
							ShipManager.Instance.BindShip(shipId, instantiatedShip);

							Vector3 spawnPos = instantiatedShip.transform.position;
							spawnPos.z -= 0.5f;

							GameObject sPlayer = uLink.Network.Instantiate(player, oPlayerPrefab, pPlayerPrefab, sPlayerPrefab, spawnPos, Quaternion.identity, 1, playerId);
							sPlayer.transform.parent = instantiatedShip.transform;
						});
					}
					else {
						Vector3 spawnPos = instantiatedShip.transform.position;
						GameObject sPlayer = uLink.Network.Instantiate(player, oPlayerPrefab, pPlayerPrefab, sPlayerPrefab, spawnPos, Quaternion.identity, 1, playerId);
						sPlayer.transform.parent = instantiatedShip.transform;
						sPlayer.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
					}
				}
				else {

				}
				
				
				//If the player exists, look at their ship
				//Look at the zone the ship is in
				//Pull back the zone information
				//Instantiate the player inside the ship

				//go.networkView.viewID;
			}
		}
	}

	public int port = 7100;
	public int maxConnections = 64;
	
	public bool cleanupAfterPlayers = true;
	public bool registerHost = false;
	public int targetFrameRate = 60;
	public bool dontDestroyOnLoad = false;
	public InstantiateOnConnected instantiateOnConnected = new InstantiateOnConnected();
	
	void Start()
	{
		Application.targetFrameRate = targetFrameRate;

		if (dontDestroyOnLoad) DontDestroyOnLoad(this);
	
		uLink.Network.InitializeServer(maxConnections, port);
	}

	void uLink_OnServerInitialized()
	{
		Debug.Log("Server successfully started on port " + uLink.Network.listenPort);
		
		if (registerHost) uLink.MasterServer.RegisterHost();
	}

	void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
	{
		if (cleanupAfterPlayers)
		{
			uLink.Network.DestroyPlayerObjects(player);
			uLink.Network.RemoveRPCs(player);
			
			// this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
			uLink.Network.RemoveInstantiates(player);
		}
	}

	void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
	{
		Debug.Log("got player");
		instantiateOnConnected.Instantiate(player);
	}
}

// (c)2011 MuchDifferent. All Rights Reserved.

using UnityEngine;
using uLink;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("TS Server/Client GUI")]
public class TSClientLogin : uLink.MonoBehaviour
{
	public bool inputName = true;
	
	public string quickText = "Play on Localhost";
	public string quickHost = "127.0.0.1";
	public int quickPort = 7100;
	
	public bool hasAdvancedMode = true;
	public string gameType = "MyUniqueGameType";
	public bool showGameLevel = false;
	
	public bool reloadOnDisconnect = false;
	
	public int targetFrameRate = 60;
	
	private string 	username, 
					password;
	
	private bool isQuickMode = true;
	
	private Vector2 scrollPosition = Vector2.zero;
	private int selectedGrid = 0;
	
	private bool isRedirected = false;
	
	public bool dontDestroyOnLoad = false;
	
	public bool lockCursor = true;
	public bool hideCursor = true;
	
	void Awake()
	{
		#if !UNITY_2_6 && !UNITY_2_6_1
		if (Application.webSecurityEnabled)
		{
			Security.PrefetchSocketPolicy(uLink.NetworkUtility.ResolveAddress(quickHost).ToString(), 843);
			Security.PrefetchSocketPolicy(uLink.MasterServer.ipAddress, 843);
		}
		#endif
		
		Application.targetFrameRate = targetFrameRate;
		
		if (dontDestroyOnLoad) DontDestroyOnLoad(this);

		Messenger.AddListener<string, string>("loginClicked", OnLogin);
	}

	void OnLogin(string username, string password) {
		new CI.Request().Send (Server.API_URL + "user?username=" + username + "&password=" + password, "GET", false, (request) => {

			Debug.Log ("Login Success");

			object o = MiniJSON.Json.Deserialize(request.ResponseText);
			Dictionary<string, object> user = (Dictionary<string, object>) o;
			string userId = (string) user["_id"];

			new CI.Request().Send (Server.API_URL + "player?userId=" + userId, "GET", false, (req) => {
				IList players = (IList) MiniJSON.Json.Deserialize(req.ResponseText);
				if (players.Count > 0) {
					IDictionary player = (IDictionary) players[0];
					string playerId = (string) player["_id"];
					string shipId = (string) player["ship_id"];

					TSClientController.Instance.PutStorageValue("playerId", playerId);
					TSClientController.Instance.PutStorageValue("shipId", shipId);

					uLink.Network.Connect(quickHost, quickPort, "", new object[]{playerId, shipId});
					GameObject.Find("UI Root (2D)").SetActive(false);
				}

			});
		});
	}
	
	void Connect(string host, int port)
	{
		/*isRedirected = false;
		
		if (inputName)
		{
			uLink.Network.Connect(host, port, "", playerName);
		}
		else
		{
			uLink.Network.Connect(host, port);
		}*/
	}
	
	void Connect(uLink.HostData host)
	{
		/*isRedirected = false;
		
		if (inputName)
		{
			uLink.Network.Connect(host, "", playerName);
			uLink.Network.Connect(host, "", {playerName, password});
		}
		else
		{
			uLink.Network.Connect(host);
		}*/
	}
	
	void uLink_OnRedirectingToServer()
	{
		isRedirected = true;
		//EnableGUI(true);
	}
	
	void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode)
	{
		isQuickMode = true;
		
		if (reloadOnDisconnect && mode != uLink.NetworkDisconnection.Redirecting && Application.loadedLevel != -1)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}


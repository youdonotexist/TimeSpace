using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class Cache
{
	static string DATA_PATH = Application.persistentDataPath + "/__cache/";
	
	public static void Init() {
		if (!false) {//Application.isWebPlayer) {
			if (!System.IO.Directory.Exists(DATA_PATH)) {
				Debug.Log("Gonna try to create directory: " + DATA_PATH);
				System.IO.Directory.CreateDirectory(DATA_PATH);	
			}
		}
	}
	
	public static void Save(string key, string val) {
		if (false) { //Application.isWebPlayer) {
			PlayerPrefs.SetString(key, val);
		}
		else {
			StreamWriter sw = new StreamWriter ( DATA_PATH + key, false);
			sw.Write(val);
			sw.Close ();
		}
	}
	
	public static string Load(string key) {
		
		if (false) {//Application.isWebPlayer) {
			return PlayerPrefs.GetString(key);
		}
		else {
			StringBuilder output = new StringBuilder(); 
			string line;
			
			try {
				StreamReader sr = new StreamReader ( DATA_PATH + key );
				line = sr.ReadLine();
				while (line != null) {
					output.Append(line);
					line = sr.ReadLine();
				}
				sr.Close();
				return output.ToString();
			}
			catch (Exception e) {
				return null;
			}	
		}
	}
	
}


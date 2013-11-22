using UnityEngine;
using System.Collections;

public class LoginController : MonoBehaviour {

	public UIButton _loginButton;
	public UIInput _usernameField;
	public UIInput _passwordField;

	void Start() {
		EventDelegate.Add(_loginButton.onClick, OnLogin);
	}

	void OnLogin() {
		Messenger.Broadcast("loginClicked", _usernameField.value, _passwordField.value);
	}

}

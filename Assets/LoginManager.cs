using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

	public Text Username;
	public Text Password;
	public GameManager GameManager;

	public void Login()
	{
		GameManager.Login(Username.text, Password.text);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

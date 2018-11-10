using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public IServer Server;

	// Use this for initialization
	void Start () {
		Server = new Server.Server("test@innopolis.ru", "123qweasd"); //TODO: remove when login screen is ready
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("GameManager").GetComponent<GameManager>().StartManager();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

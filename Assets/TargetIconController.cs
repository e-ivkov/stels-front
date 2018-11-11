using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;
using UnityEngine.UI;

public class TargetIconController : MonoBehaviour
{
	public User User;

	public GameManager GameManager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select()
	{
		GetComponent<Outline>().enabled = true;
		if (GameManager.SelectedIcon != null)
		{
			GameManager.SelectedIcon.GetComponent<Outline>().enabled = false;
		}

		GameManager.SelectedIcon = gameObject;
	}
}

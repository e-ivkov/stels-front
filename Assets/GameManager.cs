using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public IServer Server;
	public GameObject IconParent;
	public GameObject TargetIconPrefab;
	public bool updateTargets = true;
	public float UpdateDelay;
	public GameObject SelectedIcon = null;

	// Use this for initialization
	void Start () {
		Server = new Server.Server("test@innopolis.ru", "123qweasd"); //TODO: remove when login screen is ready
		StartCoroutine(UpdateTargetIcons());
	}

	IEnumerator UpdateTargetIcons()
	{
		while (updateTargets)
		{
			foreach (var icon in GameObject.FindGameObjectsWithTag("targetIcon"))
			{
				Destroy(icon);
			}
			var users = Server.GetUsers();
			foreach (var user in users)
			{
				if(!user.Alive) continue;
				var icon = Instantiate(TargetIconPrefab, IconParent.transform);
				icon.GetComponent<TargetIconController>().User = user;
				icon.GetComponent<TargetIconController>().GameManager = gameObject.GetComponent<GameManager>();
				var tex = user.Photo;
				var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
				icon.GetComponent<Image>().sprite = sprite;
				if (SelectedIcon == null)
				{
					icon.GetComponent<TargetIconController>().Select();
				}
			}
			yield return new WaitForSeconds(UpdateDelay);
		}
	}

	public void TryNeutralize()
	{
		Server.TryNeutralize(SelectedIcon.GetComponent<TargetIconController>().User);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

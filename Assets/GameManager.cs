using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Examples;
using Server;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public IServer Server;
	public GameObject IconParent;
	public GameObject TargetIconPrefab;
	public bool updateTargets = true;
	public bool sendPosition = true;
	public float UpdateDelay;
	public float PositionSendDelay;
	[HideInInspector]
	public GameObject SelectedIcon = null;
	public GameObject Player;
	public MessageController MessagePanel;

	// Use this for initialization
	void Start () {
		Server = new Server.Server("test@innopolis.ru", "123qweasd"); //TODO: remove when login screen is ready
		StartCoroutine(UpdateTargetIcons());
		StartCoroutine(SendMyPosition());
	}

	IEnumerator SendMyPosition()
	{
		while (sendPosition)
		{
			var location = Player.GetComponent<ImmediatePositionWithLocationProvider>().LocationProvider.CurrentLocation
				.LatitudeLongitude;
			Server.UpdateLocation(new Location(){latitude = location.x, longitude = location.y});
			yield return new WaitForSeconds(PositionSendDelay);
		}
	}

	private IEnumerator UpdateTargetIcons()
	{
		while (updateTargets)
		{
			var selectFirst = false;
			var users = new List<User>(Server.GetUsers());
			foreach (var icon in GameObject.FindGameObjectsWithTag("targetIcon"))
			{
				var user = icon.GetComponent<TargetIconController>().User;
				if (users.Count(u => u.Alive && u.Id == user.Id) > 0)
				{
					users.Remove(users.First(u => u.Alive && u.Id == user.Id));
				}
				else
				{
					if (SelectedIcon == icon)
					{
						selectFirst = true;
					}
					Destroy(icon);
				}
			}
			foreach (var user in users)
			{
				if(!user.Alive) continue;
				var icon = Instantiate(TargetIconPrefab, IconParent.transform);
				icon.GetComponent<TargetIconController>().User = user;
				icon.GetComponent<TargetIconController>().GameManager = gameObject.GetComponent<GameManager>();
				var tex = user.Photo;
				var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
				icon.GetComponent<Image>().sprite = sprite;
				if (SelectedIcon == null || selectFirst)
				{
					icon.GetComponent<TargetIconController>().Select();
				}
			}
			yield return new WaitForSeconds(UpdateDelay);
		}
	}

	public void TryNeutralize()
	{
		var response = Server.TryNeutralize(SelectedIcon.GetComponent<TargetIconController>().User);
		StartCoroutine(MessagePanel.ShowMessage(response));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

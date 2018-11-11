using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Examples;
using Mapbox.Utils;
using Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public IServer Server;
	public GameObject IconParent;
	public GameObject TargetIconPrefab;
	public bool updateTargets = true;
	public bool sendPosition = true;
	public bool updateEnemyLocation = true;
	public bool checkAlive = true;
	public float UpdateDelay;
	public float PositionSendDelay;
	public float EnemyLocationDelay;
	public float AliveCheckDelay;
	[HideInInspector]
	public GameObject SelectedIcon = null;
	public GameObject Player;
	public GameObject Spawner;
	public MessageController MessagePanel;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	public void Login(string username, string password)
	{
		Server = new Server.Server(username, password);
		SceneManager.LoadScene("SampleScene");
	}

	public void StartManager()
	{
		IconParent = GameObject.Find("Content");
		Player = GameObject.Find("PlayerTarget");
		MessagePanel = GameObject.Find("MessagePanel").GetComponent<MessageController>();
		Spawner = GameObject.Find("Spawner");
		var killButton = GameObject.Find("KillButton");
		killButton.GetComponent<Button>().onClick.AddListener(TryNeutralize);
		StartCoroutine(UpdateTargetIcons());
		StartCoroutine(SendMyPosition());
		StartCoroutine(UpdateEnemiesLocations());
		StartCoroutine(CheckAlive());
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
	
	IEnumerator CheckAlive()
	{
		while (checkAlive)
		{
			var alive = Server.AmIAlive();
			if(!alive)
				StartCoroutine(MessagePanel.ShowMessage("You were neutralized"));
			yield return new WaitForSeconds(AliveCheckDelay);
		}
	}

	private IEnumerator UpdateEnemiesLocations()
	{
		while (updateEnemyLocation)
		{
			var users = new List<User>(Server.GetUsers());
			var locations = users.Select(u => new Vector2d(u.Location.latitude, u.Location.longitude));
			Spawner.GetComponent<SpawnOnMap>().SpawnEnemies(locations.ToList());
			yield return new WaitForSeconds(EnemyLocationDelay);
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

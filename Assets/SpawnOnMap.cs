using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class SpawnOnMap : MonoBehaviour {

    List<GameObject> enemies;

    bool _isLocationProviderInitialized;

    ILocationProvider _locationProvider;
    ILocationProvider LocationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }

            return _locationProvider;
        }
    }
    AbstractMap map;


    // Use this for initialization
    void Start () {
        enemies = new List<GameObject>();
        LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isLocationProviderInitialized = true;
        map = LocationProviderFactory.Instance.mapManager;
    }

    //Removes enemies from the map
    private void RemoveEnemies()
    {
        enemies.ForEach(delegate (GameObject enemy)
        {
            Destroy(enemy.gameObject);
        });
        enemies = new List<GameObject>();
    }

    //Spawns enemies with the given coordinates 
    public void SpawnEnemies(List<Vector2d> locations)
    {
        if (_isLocationProviderInitialized)
        {
            RemoveEnemies();
            transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            locations.ForEach(delegate (Vector2d location)
            {
                //TODO Create an enemy from prefab and change its transform according to the location.
            });
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

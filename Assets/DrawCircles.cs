using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawCircles : MonoBehaviour {
    public int segments = 50;
    public float minRadius = 1;
    public float maxRadius = 5;
    public float speed = 5;
    LineRenderer line;
    // Creates points of the circle using given radius
    void CreatePoints(float radius)
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
    // Use this for initialization
    void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }
	// Update is called once per frame
	void Update () {
        CreatePoints(Mathf.Repeat(Time.time, (maxRadius - minRadius)/speed)*speed + minRadius);
	}
}

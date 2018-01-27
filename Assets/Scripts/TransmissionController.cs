using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransmissionController : MonoBehaviour {

	public GameObject SourcePlayer;
	public float time = 0;
	public float speed = 10;
	LineRenderer line;
	int segments = 50;

	public UnityEvent transmittedEvent;

	void Start ()
	{
		line = gameObject.GetComponent<LineRenderer>();

		//line.SetVertexCount (segments + 1);
		line.loop = true;
		//line.useWorldSpace = false;
		//CreatePoints ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (time == 0)
			time = Time.time;

		// Loop every 10 seconds for testing
		if (Time.time - time > 10)
			time = Time.time;
		
		CreatePoints ();
	}

	void CreatePoints ()
	{
		float x;
		float y;
		float z;

		float angle = 20f;

		List<Vector3> points = new List<Vector3> ();

		float xradius, yradius;
		xradius = yradius = speed * (Time.time - time);

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			points.Add(transform.position + new Vector3(x,0,z));

			angle += (360f / segments);
		}

		line.SetPositions (points.ToArray ());
	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransmissionController : MonoBehaviour {

	public GameObject SourcePlayer;
	public float time = 0;
	public float speed = 1f;
	public Color color = Color.green;

	LineRenderer line;

	static int segments = 50;
	static Vector3[] m_UnitCircle;
	static Vector3[] UnitCircle {
		get {
			if (null == m_UnitCircle)
				InitUnitCircle ();
			return m_UnitCircle;
		}
	}
	static void InitUnitCircle()
	{
		float x;
		float y;
		float radiansPerSegment = 2 * Mathf.PI / (segments+1);
		List<Vector3> points = new List<Vector3> ();

		for (int i = 0; i <= segments; i++)
		{
			float angle = i * radiansPerSegment;
			x = Mathf.Cos (angle);
			y = Mathf.Sin (angle);

			points.Add(new Vector3(x,y,-1));
		}
		m_UnitCircle = points.ToArray ();
	}

	public UnityEvent transmittedEvent;

	void Start ()
	{
		line = gameObject.GetComponent<LineRenderer>();
		line.positionCount = segments+1;
		line.loop = true;
		line.widthMultiplier = .1f;
		line.startColor = line.endColor = color;

		//line.useWorldSpace = false;
		//CreatePoints ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (time == 0)
			time = Time.time;

		// Loop for testing
		if (Time.time - time > 10)
			Destroy (gameObject);

		UpdatePoints ();
	}

	void UpdatePoints ()
	{
		Vector3[] points = new Vector3[segments+1];
		float radius = speed * (Time.time - time);

		Vector3[] unitCircle = UnitCircle;
		for (int i = 0; i < points.Length; ++i)
			points [i] = transform.position + unitCircle [i] * radius;

		line.SetPositions (points);
	}
}

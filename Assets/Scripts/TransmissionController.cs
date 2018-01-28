using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransmissionController : MonoBehaviour {

	const float TransmissionZ = 1;
	public GameObject SourcePlayer;
	public float time = 0;
	public float speed = 1f;
	public float width = .2f;
	public Color color = Color.green;

	public PlayerController.Action TransmittedAction;

	static int segments = 50;

	static void AddCirclePoints(List<Vector3> points, int segments)
	{
		float x;
		float y;
		float radiansPerSegment = 2 * Mathf.PI / (segments);

		for (int i = 0; i <= segments+1; i++)
		{
			float angle = i * radiansPerSegment;
			x = Mathf.Cos (angle);
			y = Mathf.Sin (angle);

			points.Add(new Vector3(x,y,TransmissionZ));
		}
	}

	static Mesh m_UnitRingMesh;
	static void InitUnitRingMesh()
	{

		List<Vector3> points = new List<Vector3> ();
		AddCirclePoints (points, 2*segments);
		Vector3[] vertices = points.ToArray();

		//Vector2[] uv = new Vector2[vertices.Length];

		int[] triangles = new int[3*(2*segments)];
		for (int i = 0; i < segments; ++i) {
			// Triangle 1
			triangles [6 * i + 0] = 2 * i;
			triangles [6 * i + 1] = 2 * i + 1;
			triangles [6 * i + 2] = 2 * i + 2;

			// Triangle 2
			triangles [6 * i + 3] = 2 * i + 2;
			triangles [6 * i + 4] = 2 * i + 1;
			triangles [6 * i + 5] = 2 * i + 3;

		}
			
		m_UnitRingMesh = new Mesh ();
		m_UnitRingMesh.vertices = vertices;
		//m_UnitRingMesh.uv = uv;
		m_UnitRingMesh.triangles = triangles;
	}
	void UpdateRingMesh2(float radius)
	{
		Mesh m = GetComponent<MeshFilter> ().mesh;
		m.Clear ();
		Vector3[] vertices = new Vector3[]{ new Vector3 (0, 0, 0), new Vector3 (radius, 0, 0), new Vector3 (radius, radius, 0) };
		int[] triangles = new int[]{ 0, 1, 2 };
		m.vertices = vertices;
		m.triangles = triangles;
	}
	void UpdateRingMesh(float radius)
	{
		if (null == m_UnitRingMesh)
			InitUnitRingMesh ();
		Mesh m = GetComponent<MeshFilter> ().mesh;
		bool fromScratch = false;
		if (m.vertices.Length != m_UnitRingMesh.vertices.Length) {
			fromScratch = true;
			m.vertices = new Vector3[m_UnitRingMesh.vertices.Length];
		}
		Vector3[] vertices = m.vertices;
		float innerRadius = radius - width;
		for (int i = 0; i <= segments; ++i) {
			vertices[2 * i] = radius * m_UnitRingMesh.vertices [2 * i];
			vertices[2 * i].z = TransmissionZ;

			vertices[2 * i + 1] = innerRadius * m_UnitRingMesh.vertices [2 * i + 1];
			vertices[2 * i + 1].z = TransmissionZ;
		}
		m.vertices = vertices;
		if( fromScratch )
			m.triangles = m_UnitRingMesh.triangles;
	}

	public static List<TransmissionController> TransmissionControllers = new List<TransmissionController>();
	public static void AddTransmissionController(TransmissionController tc)
	{
		TransmissionControllers.Add (tc);
	}
	public static void RemoveTransmissionController(TransmissionController tc)
	{
		TransmissionControllers.Remove (tc);
	}
		
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if (time == 0)
			time = Time.time;

		// Kill after 10 seconds
		if (Time.time - time > 10) {
			RemoveTransmissionController (this);
			Destroy (gameObject);
		}

		UpdateRingMesh (TransmissionDistance());

		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		renderer.material.color = color;
	}

	public float TransmissionDistance()
	{
		if (time == 0)
			return 0;
		return speed * (Time.time - time);
	}
}

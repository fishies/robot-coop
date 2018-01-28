using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionManager : MonoBehaviour {

	public List<TransmissionController> TransmissionControllers;
	public void AddTransmissionController(TransmissionController tc)
	{
		tc.manager = this;
		TransmissionControllers.Add (tc);
	}
	public void RemoveTransmissionController(TransmissionController tc)
	{
		TransmissionControllers.Remove (tc);
	}

	// Use this for initialization
	void Start () {
		TransmissionControllers = new List<TransmissionController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[System.Serializable]
	public class Action {
		public string Button; //name of button in unity input manager
		public GameObject SourcePlayer;
		public bool Seen;
	}

	public GameObject Transmission;

	public string[] Buttons;


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		foreach (var b in Buttons)
			CheckButton (b);
		
		foreach (var transmission in TransmissionController.TransmissionControllers) {
			if (transmission.TransmittedAction.SourcePlayer == gameObject || transmission.TransmittedAction.Seen)
				continue;
			float distanceToSource = Vector3.Distance (transform.position, transmission.transform.position);
			float transmissionDistance = transmission.TransmissionDistance ();
			if (transmissionDistance > distanceToSource && transmissionDistance - distanceToSource < .1)
				ActivateButton (transmission.TransmittedAction.Button);
		}
		TransmissionController[] transmissions = FindObjectsOfType<TransmissionController> ();
	}

	void CheckButton(string button)
	{
		if (Input.GetButtonDown (button)) {

			GetComponent<PlayerController> ().ActivateButton (button);

			Action a = new Action ();
			a.Button = button;
			a.SourcePlayer = gameObject;

			GameObject t = Instantiate (Transmission, transform.position, Quaternion.identity);
			TransmissionController transmissionController = t.GetComponent<TransmissionController> ();
			transmissionController.TransmittedAction = a;

			TransmissionController.AddTransmissionController (transmissionController);
		}
	}

	public void ActivateButton(string button)
	{
		switch (button) {
		case "Jump":
			Jump ();
			break;
		case "Fire1":
			Fire1 ();
			break;
		}
	}

	public void Fire1(){
	}

	public void Jump(){
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 100));
	}

}

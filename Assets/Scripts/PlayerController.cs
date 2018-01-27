using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject Transmission;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Debug.Log ("Button down: Fire1");
			Instantiate (Transmission, transform.position, Quaternion.identity);
		}
			
	}
}

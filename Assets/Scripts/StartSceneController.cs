using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneController : MonoBehaviour {

	public string SceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//for (int i = 1; i < 10; ++i) {
		//	if (Input.GetButtonDown (string.Format ("joystick button {0}", i)))
		//		Debug.LogFormat ("Joystick button {0}", i);
		//}
		//return;
		if (Input.GetButtonDown("Cancel"))
			Application.Quit ();

		if (Input.GetButtonDown("Start") || Input.GetButtonDown("Start2")) {
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);
		}	
	}
}

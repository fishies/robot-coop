using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneController : MonoBehaviour {

	public string SceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel"))
			Application.Quit ();

		if (Input.GetButtonDown("Start") || Input.GetButtonDown("Start2")) {
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);
		}
	}
}

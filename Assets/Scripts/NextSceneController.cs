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
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit ();

        if( InControl.InputManager.ActiveDevice.GetControl(InControl.InputControlType.Start) )
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);
	}
}

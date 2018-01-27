using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActions : MonoBehaviour {

    [System.Serializable]
    public class Action {
        public string Player1Button; //name of button in unity input manager
        public string Player2Button;
        public UnityEvent Function;
    }

    public Action[] actions;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        foreach(Action action in actions)
        {
            if((action.Player1Button != "" && Input.GetButtonDown(action.Player1Button))
            || (action.Player2Button != "" && Input.GetButtonDown(action.Player2Button)))
            {
                action.Function.Invoke();
            }
        }
    }
    
    public void foo () {
        Debug.Log("foo called");
    }
}

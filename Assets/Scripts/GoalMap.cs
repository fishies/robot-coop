using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalMap : MonoBehaviour {

    //every object in this array must be touching the goal tiles in order for us to advance to the next scene
    public Collider2D [] requirements;
    //name of next scene
    public string sceneName;


    Collider2D collider;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
    }
    
    // Update is called once per frame
    void Update () {
        bool goalReached = true;
        foreach(var req in requirements) {
            goalReached &= collider.IsTouching(req);
        }
        if(goalReached) {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}

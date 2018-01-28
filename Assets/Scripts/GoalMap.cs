using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalMap : MonoBehaviour {

    //every object in this array must be touching the goal tiles in order for us to advance to the next scene
    public Collider2D [] requirements;
    //name of next scene
    public string sceneName;

    AudioSource audio;
    Collider2D collider;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
    }
    
    // Update is called once per frame
    void Update () {
        bool goalReached = true;
        foreach(var req in requirements) {
            goalReached &= collider.IsTouching(req);
        }
        if(goalReached && !audio.isPlaying) {
            foreach(var c in requirements) {
                Destroy(c.gameObject.GetComponent<PlayerController>());
            }
            audio.Play();
            Invoke("nextScene",audio.clip.length);
        }
    }

    void nextScene() {
        SceneManager.LoadSceneAsync(sceneName);
    }
}

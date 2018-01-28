using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    //always place a moving platform in its most bottom/left position
    public Vector2 offset;
    Vector2 origin;

    Collider2D collider;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
        origin = transform.position;
    }
    
    // Update is called once per frame
    void Update () {
        transform.Translate((origin+offset).normalized * Time.deltaTime);
        //need to also move player with platform!
    }
}

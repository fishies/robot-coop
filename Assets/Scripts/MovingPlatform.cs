using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector2 minOffset;
    public Vector2 maxOffset;

    Collider2D collider;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
    }
    
    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.up * Time.deltaTime);
        //need to also move player with platform!
    }
}

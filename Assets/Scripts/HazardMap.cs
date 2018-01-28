using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMap : MonoBehaviour {
    public Vector2 spawnPoint;
    Collider2D collider;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
    }
    
    // Update is called once per frame
    void Update () {
    }

    void OnCollisionEnter2D (Collision2D c) {
        c.rigidbody.position = spawnPoint;
    }
}

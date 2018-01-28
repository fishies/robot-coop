using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMap : MonoBehaviour {
    public Vector2 spawnPoint;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
    }

    void OnCollisionEnter2D (Collision2D c) {
        c.rigidbody.position = spawnPoint;
        c.gameObject.GetComponent<AudioSource>().PlayOneShot(c.gameObject.GetComponent<PlayerController>().warpSound);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

[RequireComponent(typeof(UnityArmatureComponent))]
public class AnimatorTest : MonoBehaviour {

    private UnityArmatureComponent armature;
	// Use this for initialization
	void Start () {
        armature = GetComponent<UnityArmatureComponent>();
        armature.animation.Play("walk");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

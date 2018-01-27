using UnityEngine;
using System.Collections;
using DragonBones;

public class TestAnimation : MonoBehaviour {
    void Start() {
        UnityFactory.factory.LoadDragonBonesData("simple_rig/SimpleDude_ske"); // DragonBones file path (without suffix) 
        UnityFactory.factory.LoadTextureAtlasData("simple_rig/SimpleDude_tex"); //Texture atlas file path (without suffix) 
        // Create armature. 
        var armatureComponent = UnityFactory.factory.BuildArmatureComponent("Robo1");
        // Input armature name 

        // Play animation. 
       armatureComponent.animation.Play("walk");


        // Change armatureposition. 
        armatureComponent.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
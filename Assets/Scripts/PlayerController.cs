using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[System.Serializable]
	public class Action {
		public ActionEnum Button; //name of button in unity input manager
		public GameObject SourcePlayer;
		public bool Seen;
	}

	public enum ActionEnum {
		Jump,
		Drill
	}

    public int PlayerIndex = 0;
	public TransmissionManager transmissionManager;
	public GameObject Transmission;

	public Color TransmissionColor = Color.green;
	public int MirrorHack = 1;

	Collider2D collider;
	Rigidbody2D rb;

	AudioSource audio;
	public AudioClip walkSound;
	public AudioClip jumpSound;
	public AudioClip warpSound;
	public AudioClip drillSound;
	public AudioClip sendSound;
	public AudioClip recvSound;

	float m_DrillStartTime = 0;

	enum AnimationEnum
	{
		None,
		Idle,
		Walk,
		Drill
	}
	AnimationEnum animation;
	string AnimationEnumToName(AnimationEnum a)
	{
		switch (a) {
		case AnimationEnum.None:
			return string.Empty;
		case AnimationEnum.Idle:
			return "idle";
		case AnimationEnum.Walk:
			return "walk";
		case AnimationEnum.Drill:
			return "drill";
		}
		return string.Empty;
	}
	void SetAnimation(AnimationEnum a)
	{
		if (a == animation)
			return;
		
		animation = a;

		var armatureComponent = GetComponent<DragonBones.UnityArmatureComponent> ();
		if (null == armatureComponent)
			return;
		
		string animationName = AnimationEnumToName (animation);
		if( string.IsNullOrEmpty(animationName) )
			armatureComponent.animation.Stop();
		else
			armatureComponent.animation.Play(animationName);
	}

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider2D>();
		rb = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		// Stop drilling after x seconds
		AnimationEnum nextAnimation = animation;
		if (animation == AnimationEnum.Drill) {
			const float SecondsToDrill = 3f;
			if (Time.time - m_DrillStartTime > SecondsToDrill) {
				m_DrillStartTime = 0;
				nextAnimation = AnimationEnum.Idle;
			}
		}


        float movementX = 0;
        float movementY = 0;
        bool jump = false;
        bool drill = false;
        // Use last device which provided input.
        if (InControl.InputManager.Devices.Count == 0)
            return;
        if (PlayerIndex < InControl.InputManager.Devices.Count)
        {
            InControl.InputDevice device = InControl.InputManager.Devices[PlayerIndex];
            movementX = device.LeftStickX;
            if (device.DPadLeft)
                movementX = -1;
            else if (device.DPadRight)
                movementX = 1;
            movementY = device.LeftStickY;
            if (device.DPadUp)
                movementY = 1;
            else if (device.DPadDown)
                movementY = -1;
            jump = device.Action1.WasPressed;
            drill = device.Action3.WasPressed;
        }
        else
        {
            InControl.InputDevice device = InControl.InputManager.Devices[0];
            movementX = device.RightStickX;
            movementY = device.RightStickY;
            jump = device.Action2.WasPressed;
            drill = device.Action4.WasPressed;

        }

        // Move if able
        if (nextAnimation != AnimationEnum.Drill) {
			rb.velocity = new Vector2(3.0f*movementX,rb.velocity.y);

			if (rb.velocity.x == 0)
				nextAnimation = AnimationEnum.Idle;
			else
				nextAnimation = AnimationEnum.Walk;
		}

		// Look the correct way
		if (rb.velocity.x < 0)
			transform.localScale = new Vector3(MirrorHack*-1*Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
		else if (rb.velocity.x > 0)
			transform.localScale = new Vector3(MirrorHack*Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);

		// Apply the animation
		SetAnimation (nextAnimation);

		// Walking sound
        if(collider.Raycast(Vector2.down,new RaycastHit2D[]{new RaycastHit2D()},collider.bounds.extents.y+0.125f) > 0
       	&& movementX != 0.0f && !audio.isPlaying)
       		audio.PlayOneShot(walkSound);


        //if( inputDevice.GetControl(InControl.InputControlType))
        // Rotate target object with left stick.
        //transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * inputDevice.LeftStickX, Space.World);
        //transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * inputDevice.LeftStickY, Space.World);

        // Check direct inputs
        CheckButton(ActionEnum.Jump, jump);
        CheckButton(ActionEnum.Drill, drill);

		// Check for received transmissions
		if (null == transmissionManager.TransmissionControllers)
			return;
		foreach (var transmission in transmissionManager.TransmissionControllers) {
			if (transmission.TransmittedAction.SourcePlayer == gameObject || transmission.TransmittedAction.Seen)
				continue;
			float distanceToSource = Vector3.Distance (transform.position, transmission.transform.position);
			float transmissionDistance = transmission.TransmissionDistance ();
			if (transmissionDistance > distanceToSource) {
				//Debug.LogFormat ("See transmission: {0} > {1}", transmissionDistance, distanceToSource);
				audio.PlayOneShot(recvSound, .75f);
				ActivateButton (transmission.TransmittedAction.Button);
				transmission.TransmittedAction.Seen = true;
			}
		}
	}

    void CheckButton()
    {

    }

	void CheckButton(ActionEnum action, bool pressed)
	{
		if (pressed) {
			//GetComponent<PlayerController> ().ActivateButton (action);

			audio.PlayOneShot(sendSound);

			Action a = new Action ();
			a.Button = action;
			a.SourcePlayer = gameObject;

			GameObject t = Instantiate (Transmission, transform.position, Quaternion.identity);
			TransmissionController transmissionController = t.GetComponent<TransmissionController> ();
			transmissionController.TransmittedAction = a;
			transmissionController.color = TransmissionColor;

			transmissionManager.AddTransmissionController (transmissionController);
		}
	}


	public void ActivateButton(ActionEnum a)
	{
		switch (a) {
		case ActionEnum.Jump:
			Jump ();
			break;
		case ActionEnum.Drill:
			Drill ();
			break;
		}
	}

	public void Drill(){
		m_DrillStartTime = Time.time;
		SetAnimation (AnimationEnum.Drill);
		audio.PlayOneShot(drillSound);
	}

	public void Jump(){
		if(collider.Raycast(Vector2.down,new RaycastHit2D[]{new RaycastHit2D()},collider.bounds.extents.y+0.125f) > 0)
		{
			rb.AddForce(Vector2.up * 320.0f);
			audio.PlayOneShot(jumpSound,2.0f);
		}
	}
}

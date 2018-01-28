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

	public GameObject Transmission;

	public string HorizontalAxisControl = "Horizontal";
	public string JumpButtonControl = "Jump";
	public string DrillButtonControl = "Drill";
	public Color TransmissionColor = Color.green;

	BoxCollider2D collider;
	Rigidbody2D rb;

	enum AnimationEnum
	{
		None,
		Idle,
		Walk
	}
	AnimationEnum animation;
	string AnimationEnumToName(AnimationEnum a)
	{
		switch (a) {
		case AnimationEnum.None:
			return "";
		case AnimationEnum.Idle:
			return "idle";
		case AnimationEnum.Walk:
			return "walk";
		}
		return "";
	}
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(8.0f*Input.GetAxis(HorizontalAxisControl),rb.velocity.y);
		if (rb.velocity.x > 0)
			transform.localScale = new Vector3 (1, 1, 1);
		else if (rb.velocity.x < 0)
			transform.localScale = new Vector3 (-1, 1, 1);

		var armatureComponent = GetComponent<DragonBones.UnityArmatureComponent> ();
		if (null != armatureComponent) {
			AnimationEnum nextAnimation = AnimationEnum.None;
			if (rb.velocity.x == 0)
				nextAnimation = AnimationEnum.Idle;
			else
				nextAnimation = AnimationEnum.Walk;
			if (animation != nextAnimation) {
				animation = nextAnimation;
				string animationName = AnimationEnumToName (animation);
				if( string.IsNullOrEmpty(animationName) )
					GetComponent<DragonBones.UnityArmatureComponent> ().animation.Stop();
				else
					GetComponent<DragonBones.UnityArmatureComponent> ().animation.Play(animationName);
			}
		}

		CheckButton (JumpButtonControl, ActionEnum.Jump);
		CheckButton (DrillButtonControl, ActionEnum.Drill);

		foreach (var transmission in TransmissionController.TransmissionControllers) {
			if (transmission.TransmittedAction.SourcePlayer == gameObject || transmission.TransmittedAction.Seen)
				continue;
			float distanceToSource = Vector3.Distance (transform.position, transmission.transform.position);
			float transmissionDistance = transmission.TransmissionDistance ();
			if (transmissionDistance > distanceToSource) {
				//Debug.LogFormat ("See transmission: {0} > {1}", transmissionDistance, distanceToSource);
				ActivateButton (transmission.TransmittedAction.Button);
				transmission.TransmittedAction.Seen = true;
			}
		}
		TransmissionController[] transmissions = FindObjectsOfType<TransmissionController> ();
	}

	void CheckButton(string button, ActionEnum action)
	{
		if (Input.GetButtonDown (button)) {
			Debug.LogFormat ("Button pressed: {0}", button);
			GetComponent<PlayerController> ().ActivateButton (action);

			Action a = new Action ();
			a.Button = action;
			a.SourcePlayer = gameObject;

			GameObject t = Instantiate (Transmission, transform.position, Quaternion.identity);
			TransmissionController transmissionController = t.GetComponent<TransmissionController> ();
			transmissionController.TransmittedAction = a;
			transmissionController.color = TransmissionColor;

			TransmissionController.AddTransmissionController (transmissionController);
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
	}

	public void Jump(){
		if(collider.Raycast(Vector2.down,new RaycastHit2D[]{new RaycastHit2D()},collider.bounds.extents.y+0.125f) > 0)
			rb.AddForce(Vector2.up * 320.0f);
	}
}

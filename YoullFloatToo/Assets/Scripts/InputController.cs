// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class InputController : MonoBehaviour {

	PlayerMotor motor;

	void Start () {
		motor = GetComponent<PlayerMotor>();
	}

	void Update () {
		ProcessInput();
	}

	void ProcessInput () {
		Move();
		Rotate();
	}

	void Move () {
		Vector3 velocity = (Input.GetAxis("Vertical") * transform.forward);
		motor.Move(velocity);
	}

	void Rotate () {
		Vector3 angularVelocity = (Input.GetAxis("Horizontal") * transform.up);
		motor.Rotate(angularVelocity);
	}
}
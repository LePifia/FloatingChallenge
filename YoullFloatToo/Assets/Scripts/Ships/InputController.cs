using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class InputController : NetworkBehaviour {

	PlayerMotor motor;

	public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner){return;}
        
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsOwner){return;}
    }

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

		if (IsOwner){
			Vector3 velocity = (Input.GetAxis("Vertical") * transform.forward);
			motor.Move(velocity);
		}
		
	}

	void Rotate () {
		if (IsOwner){
			Vector3 angularVelocity = (Input.GetAxis("Horizontal") * transform.up);
			motor.Rotate(angularVelocity);
		}
		
	}
}
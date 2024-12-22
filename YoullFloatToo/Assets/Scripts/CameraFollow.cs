// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float distance = 10;
	[SerializeField] float height = 5;
	[SerializeField] Vector3 lookOffset = new Vector3(0, 1, 0);
	[SerializeField] float cameraSpeed = 10;
	[SerializeField] float rotationSpeed = 10;
	
	Vector3 lookPosition;
	Vector3 relativePosition;
	Quaternion rotation;
	Vector3 targetPosition;

	void FixedUpdate () { 
		FollowPlayer();
	}

	void FollowPlayer () {
		if (target) {
			lookPosition = target.position + lookOffset;
			relativePosition = lookPosition - transform.position;
			rotation = Quaternion.LookRotation(relativePosition);

			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
			targetPosition = target.transform.position + target.transform.up * height - target.transform.forward * distance;
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * cameraSpeed);
		}
	}
}
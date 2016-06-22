using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[Header("References")]
	//Private
	[SerializeField] PlayerController owner;
	[SerializeField] Transform target;
	[SerializeField] Volleyball ball;
	[SerializeField] Transform cameraPivot;
	[SerializeField] Transform cameraPosObject;

	//Public
	public Transform CameraPosObject { get { return cameraPosObject; } }


	[Header("Camera")]
	//Private
	[SerializeField] State state = State.LookAtTarget;
	float minDistance = 0f;
	float maxDistance = 100f;
	[SerializeField] float cameraDistance = 0.1f;
	[SerializeField] float swivelSpeed = 3f;
	[SerializeField] float stiffness = 5f;
	float viewAngleX;
	float viewAngleY;

	//Public
	public float CameraDistance
	{
		get { return cameraDistance; }
		set
		{
			value = Mathf.Clamp(value, minDistance, maxDistance);
			cameraDistance = value;
		}
	}

	public enum State
	{
		PlayerPivot,
		LookAtTarget,
		Free,
		Static
	}


	void Start()
	{
		if (!owner) { print("Warning: No camera owner assigned to " + transform.root.name); }
		viewAngleX = cameraPivot.eulerAngles.y;
		viewAngleY = cameraPivot.eulerAngles.x;
	}

	void FixedUpdate()
	{
		switch (state)
		{
			case State.PlayerPivot:
				if (owner && cameraPivot && cameraPosObject)
				{
					float camHorizontalInput = Input.GetAxis("Camera Horizontal");
					float camVerticalInput = Input.GetAxis("Camera Vertical");

					viewAngleX += camHorizontalInput * Time.deltaTime * 50f;
					viewAngleY += -camVerticalInput * Time.deltaTime * 50f;

					Vector3 camRotation = new Vector3(viewAngleY, viewAngleX, 0f);

					cameraPivot.rotation = Quaternion.Euler(camRotation);

					Vector3 targetPos = cameraPosObject.position;
					Quaternion targetRot = cameraPivot.rotation;

					transform.position = Vector3.Lerp(transform.position, targetPos, stiffness * Time.deltaTime);
					transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, stiffness * Time.deltaTime);
				}
				else
				{
					print("Missing reference in camera");
				}
				break;
			case State.LookAtTarget:
				if (target)
				{
					Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
					transform.forward = Vector3.Lerp(transform.forward, dirToTarget, Time.deltaTime * swivelSpeed);
				}
				break;
			case State.Free:
				break;
			case State.Static:
				break;
		}
	}
}
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
	float maxDistance = 5;
	[SerializeField] float cameraDistance = 0.5f;
	[SerializeField] float swivelSpeed = 3f;
	[SerializeField] float stiffness = 5f;
	float viewAngleX;
	float viewAngleY;
	[SerializeField] float height = 0f;
	float heightRange = 1f;

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

	public float Height
	{
		get { return Mathf.Clamp(height, -heightRange, heightRange); }
		set
		{
			value = Mathf.Clamp(value, -heightRange, heightRange);
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
					//Mouse input
					float camHorizontalInput = Input.GetAxis("Camera Horizontal");
					float camVerticalInput = Input.GetAxis("Camera Vertical");

					viewAngleX += camHorizontalInput * owner.MouseSensitivity * Time.deltaTime;
					viewAngleY += -camVerticalInput * owner.MouseSensitivity * Time.deltaTime;

					Vector3 camRotation = new Vector3(viewAngleY, viewAngleX, 0f);

					//Map input to camera rotation
					cameraPivot.rotation = Quaternion.Euler(camRotation);

					//Assign position and rotation
					Vector3 targetPos = cameraPosObject.position;
					Quaternion targetRot = cameraPivot.rotation;

					//Smooth (stiffness) and set position and rotation
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

	void Update()
	{
		switch (state)
		{
			case State.PlayerPivot:
				//Camera height
				Vector3 camPos = cameraPivot.transform.localPosition;
				camPos.y = Height;
				cameraPivot.transform.localPosition = camPos;

				//Camera distance
				Vector3 pivotToCam = transform.position - cameraPivot.transform.position;
				pivotToCam.Normalize();
				transform.position = Vector3.Lerp(cameraPivot.position, cameraPivot.position + (pivotToCam * maxDistance), CameraDistance);
				break;
		}
	}
}
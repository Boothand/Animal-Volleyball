using UnityEngine;
using RootMotion.Dynamics;

public class PlayerController : MonoBehaviour
{
	//Private
	[Header("References")]
	[SerializeField] protected PuppetMaster puppet;
	[SerializeField] protected Volleyball ball;
	[SerializeField] protected PlayerCamera cam;
	protected Rigidbody rb;
	protected Animator anim;

	[Header("Movement")]
	//Private
	[SerializeField] protected float moveSpeed = 1f;
	protected bool moving;
	Vector3 moveVector;

	[Header("Mouse/Look")]
	//Private
	protected float viewAngleX;
	protected float viewAngleY;
	protected float camHorizontalInput;
	protected float camVerticalInput;
	protected float mouseSensitivity = 80f;

	//Public
	//public Transform CameraPivot { get { return cameraPivot; } }
	//public Transform CameraPosObject { get { return cameraPosObject; } }
	public float CamHorizontal { get { return camHorizontalInput; } }
	public float CamVertical { get { return camVerticalInput; } }
	public float MouseSensitivity
	{
		get { return mouseSensitivity; }
		set
		{
			value = Mathf.Clamp(value, 20, 200);
			mouseSensitivity = value;
		}
	}

	void Start()
	{
		BaseStart();
	}

	protected virtual void BaseStart()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();

		if (!puppet) { print("Puppetmaster not assigned"); }
		if (!ball) { ball = GameObject.FindObjectOfType<Volleyball>(); }
		if (!cam) { print("Camera not assigned"); }
		//if (!cameraPivot) { cameraPivot = transform.FindChild("Camera Pivot Point"); }
		//if (!cameraPosObject && cameraPivot) { cameraPosObject = cameraPivot.FindChild("Camera Position Object"); }
	}

	protected virtual void MovePhysics()
	{
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputZ = Input.GetAxisRaw("Vertical");

		Vector3 forwardDir = transform.forward;

		if (cam)
		{
			forwardDir = cam.transform.forward;
		}

		//if (ball)	//And if jumping?
		//{
				
		//}

		forwardDir.y = transform.forward.y;

		Vector3 sideDir = Quaternion.AngleAxis(90, Vector3.up) * forwardDir;

		moveVector = (forwardDir * inputZ) + (sideDir * inputX);
		moveVector.Normalize();
		Debug.DrawRay(transform.position, moveVector);
		rb.MovePosition(transform.position + moveVector * moveSpeed * Time.deltaTime);
	}

	protected virtual void Move()
	{
		if (moveVector.magnitude > 0.001f)
		{
			moving = true;
		}
		else
		{
			moving = false;
		}

		//Facing direction when moving
		if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
		{
			Vector3 forwardDir = transform.forward;
			if (cam)
			{
				forwardDir = cam.transform.forward;
				forwardDir.y = cam.transform.eulerAngles.y;
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(forwardDir), Time.deltaTime * 5f);
		}

		if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
		{
			Vector3 sideDir = transform.right;
			if (cam)
			{
				sideDir = cam.transform.right;
				sideDir.y = cam.transform.eulerAngles.y;
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(sideDir), Time.deltaTime * 5f);
		}
	}

	protected virtual void BaseFixedUpdate()
	{
		MovePhysics();
	}

	protected virtual void BaseUpdate()
	{
		Move();
	}

	void FixedUpdate()
	{
		BaseFixedUpdate();
	}

	void Update()
	{
		BaseUpdate();
	}
}
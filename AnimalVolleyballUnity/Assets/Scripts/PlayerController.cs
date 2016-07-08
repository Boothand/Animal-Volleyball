using UnityEngine;
using RootMotion.Dynamics;

public class PlayerController : MonoBehaviour
{
	//Private
	[Header("References")]
	protected Rigidbody rb;
	protected Animator anim;
	[SerializeField] protected PuppetMaster puppet;
	[SerializeField] protected PlayerCamera cam;

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

	void Awake()
	{
		BaseAwake();
	}

	void Start()
	{
		BaseStart();
	}

	protected virtual void BaseAwake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}

	protected virtual void BaseStart()
	{
		if (!puppet) { print("Puppetmaster not assigned"); }
		if (!cam) { print("Camera not assigned"); }
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

		forwardDir.y = transform.forward.y;

		Vector3 sideDir = Quaternion.AngleAxis(90, Vector3.up) * forwardDir;

		moveVector = (forwardDir * inputZ) + (sideDir * inputX);
		moveVector.Normalize();

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

	protected virtual void SyncAnimationVars()
	{
		anim.SetFloat("Forward", Input.GetAxis("Vertical"));
		anim.SetFloat("Side", Input.GetAxis("Horizontal"));
	}

	protected virtual void BaseFixedUpdate()
	{
		MovePhysics();
	}

	protected virtual void BaseUpdate()
	{
		Move();
		SyncAnimationVars();
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
using UnityEngine;

public class Volleyball : MonoBehaviour
{
	Rigidbody rb;
	[SerializeField] Vector3 startDirection;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			rb.AddForce(startDirection);
		}

	}
}
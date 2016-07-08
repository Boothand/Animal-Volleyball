using UnityEngine;

public class Volleyball : MonoBehaviour
{
	Rigidbody rb;
	PlayerController lastPlayerTouched;
	[SerializeField] Vector3 testDir;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			rb.AddForce(testDir);
		}

	}
}
using UnityEngine;

public class BallDetectionBox : MonoBehaviour
{
	//Private
	Volleyball ball;
	[SerializeField] bool ballInBox;

	//Public
	public Volleyball Ball { get { return ball; } }
	public bool BallInBox { get { return ballInBox; } }

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<Volleyball>())
		{
			ball = col.GetComponent<Volleyball>();
			ballInBox = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.GetComponent<Volleyball>())
		{
			ball = null;
			ballInBox = false;
		}
	}
}
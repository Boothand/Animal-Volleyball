using UnityEngine;

public class BallDetectionBox : MonoBehaviour
{
	//Private
	Volleyball ball;
	[SerializeField] bool hasBall;

	//Public
	public Volleyball Ball { get { return ball; } }
	public bool HasBall { get { return hasBall; } }

	void OnTriggerEnter(Collider col)
	{
		if (col.GetComponent<Volleyball>())
		{
			ball = col.GetComponent<Volleyball>();
			hasBall = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.GetComponent<Volleyball>())
		{
			ball = null;
			hasBall = false;
		}
	}
}
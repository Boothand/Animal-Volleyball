using UnityEngine;
using RootMotion.FinalIK;

public class VolleyballPlayer : PlayerController
{
	[Header("References")]
	//Private
	[SerializeField] protected Volleyball ball;
	[SerializeField] protected BallDetectionBox setterBox;
	[SerializeField] protected BallDetectionBox diggerBox;
	BipedIK ik;

	[Header("Actions")]
	//Private
	[SerializeField] float power;
	float animPower;
	float setReleaseIterator;
	float maxPower = 5f;
	float passCancelTime = 1f;
	[SerializeField] float passCancelTimer;
	PassState passState = PassState.None;

	//Public
	enum PassState
	{
		None,
		Charging,
		Release
	}

	[Header("Animation")]
	//Private
	float handIKweight;
	[SerializeField] float WEIGHT_HAND_SET = 0.3f;
	float bodyIKweight;
	[SerializeField] float WEIGHT_BODY_SET = 0.3f;
	float headIKweight;
	[SerializeField] float WEIGHT_HEAD_IDLE = 0.3f;


	protected override void BaseAwake()
	{
		base.BaseAwake();
		ik = GetComponent<BipedIK>();

		headIKweight = WEIGHT_HEAD_IDLE;
	}

	protected override void BaseStart()
	{
		base.BaseStart();

		if (!ball) { print("Ball not assigned."); }
		else
		{
			ik.solvers.leftHand.target = ball.transform;
			ik.solvers.rightHand.target = ball.transform;

			ik.solvers.lookAt.target = ball.transform;
			//ik.solvers.spine.target = ball.transform;
		}
	}

	protected override void SyncAnimationVars()
	{
		base.SyncAnimationVars();

		anim.SetFloat("Power", power);

		//Inverse Kinematics
		float lhandWeight = ik.solvers.leftHand.IKPositionWeight;
		float rhandWeight = ik.solvers.rightHand.IKPositionWeight;
		float bodyWeight = ik.solvers.lookAt.bodyWeight;
		float headWeight = ik.solvers.lookAt.headWeight;

		ik.solvers.leftHand.IKPositionWeight = Mathf.MoveTowards(lhandWeight, handIKweight, Time.deltaTime * 2f);
		ik.solvers.rightHand.IKPositionWeight = Mathf.MoveTowards(rhandWeight, handIKweight, Time.deltaTime * 2f);
		ik.solvers.lookAt.bodyWeight = Mathf.MoveTowards(bodyWeight, bodyIKweight, Time.deltaTime * 2f);
		ik.solvers.lookAt.headWeight = Mathf.MoveTowards(headWeight, headIKweight, Time.deltaTime * 2f);
		//print(headIKweight);
	}

	protected override void BaseFixedUpdate()
	{
		base.BaseFixedUpdate();
	}

	protected override void BaseUpdate()
	{
		base.BaseUpdate();

		Cursor.lockState = CursorLockMode.Locked;

		switch (passState)
		{
			case PassState.None:
				if (Input.GetButtonDown("Pass"))
				{
					//Set the correct hand pose for the situation ('set' only for now)
					anim.SetTrigger("Set");

					//Inverse kinematics
					if (ball)
					{
						ik.solvers.leftHand.target = ball.transform;
						ik.solvers.rightHand.target = ball.transform;
					}

					handIKweight = WEIGHT_HAND_SET;
					bodyIKweight = WEIGHT_BODY_SET;
					headIKweight = 1f;
					
					passState = PassState.Charging;
				}
				break;
			case PassState.Charging:
				//If you've released button when charging, and press it down again, reset.
				if (Input.GetButtonDown("Pass"))
				{
					handIKweight = WEIGHT_HAND_SET;
					bodyIKweight = WEIGHT_BODY_SET;
					headIKweight = 1f;
					passCancelTimer = 0f;
					anim.SetBool("Cancelled", false);
					anim.SetTrigger("Set"); //Only set for now
				}

				//Increase power while holding the button
				if (Input.GetButton("Pass"))
				{
					power += Time.deltaTime * maxPower; //1 second

					if (power > maxPower)
					{
						power = maxPower;
					}
				}

				//Actually shoot the ball somewhere:
				if (!Input.GetButton("Pass") &&
					(setterBox.HasBall ||
					diggerBox.HasBall) )
				{
					if (setterBox.HasBall)
					{
						//Set:
						//Set IK and trigger release animation
						handIKweight = 0f;
						bodyIKweight = 0f;
						headIKweight = WEIGHT_HEAD_IDLE;
						anim.SetTrigger("Release");
					}
					else if (diggerBox.HasBall)
					{
						//Dig:
						//Set IK and trigger release animation
					}

					passState = PassState.Release;
				}
				else if (!Input.GetButton("Pass"))
				{
					//Cancel the animation if enough time passes with no results, when not holding button
					passCancelTimer += Time.deltaTime;

					if (passCancelTimer > passCancelTime)
					{
						power = 0f;
						handIKweight = 0f;
						bodyIKweight = 0f;
						headIKweight = WEIGHT_HEAD_IDLE;
						passCancelTimer = passCancelTime;

						anim.SetBool("Cancelled", true);
					}
				}

				break;
			case PassState.Release:

				break;
		}
	}
}
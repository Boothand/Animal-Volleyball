using UnityEngine;

public class VolleyballPlayer : PlayerController
{
	[Header("References")]
	//Private
	[SerializeField] protected BallDetectionBox setterBox;
	[SerializeField] protected BallDetectionBox diggerBox;

	[Header("Actions")]
	//Private
	[SerializeField] bool holdingPassButton;
	[SerializeField] bool bumping;
	[SerializeField] float power;
	float animPower;
	float setReleaseIterator;
	float maxPower = 1f;

	//Public
	enum SetState
	{
		None,
		Charging,
		Release
	}

	SetState setState = SetState.None;

	protected override void BaseStart()
	{
		base.BaseStart();
	}

	protected override void BaseFixedUpdate()
	{
		base.BaseFixedUpdate();

		float setReleaseTarget = 0f;

		switch (setState)
		{
			case SetState.None:
				setReleaseTarget = 0f;

				//The frame you first press the button
				if (Input.GetButton("Pass") && !holdingPassButton
					&& setterBox.BallInBox)
				{
					setState = SetState.Charging;

					//"Set" animation on hands
					anim.SetTrigger("Set");

					//IK set hands to target volleyball
					//here
				}
				break;
			case SetState.Charging:
				//While you hold the button
				if (Input.GetButton("Pass"))
				{
					power += Time.deltaTime * 5f;
					holdingPassButton = true;

					if (power > maxPower)
					{
						power = maxPower;
						setState = SetState.Release;
					}
				}
				else
				{
					setState = SetState.Release;
				}
				break;
			case SetState.Release:
				//When you release the button
				if (setReleaseIterator <= 0.01f)
				{
					if (!moving)
					{
						anim.SetTrigger("Set Release Still");
					}
				}

				setReleaseTarget = 1f;

				if (setReleaseIterator > 0.99f)
				{
					print("Went into none");
					setState = SetState.None;
				}

				//Send the ball somewhere, if the hands hit it


				power = 0f;
				break;
		}

		if (Input.GetButtonUp("Pass") && holdingPassButton)
		{
			holdingPassButton = false;
		}

		setReleaseIterator = Mathf.MoveTowards(setReleaseIterator, setReleaseTarget, Time.deltaTime * 5f);

		anim.SetFloat("SetIterator", setReleaseIterator);
	}

	protected override void SyncAnimatorVars()
	{
		base.SyncAnimatorVars();

		//Power
		if (setState == SetState.Charging)
		{
			animPower = power;
		}
		else
		{
			animPower = Mathf.MoveTowards(animPower, power, Time.deltaTime * 1.5f);
		}

		anim.SetFloat("Power", animPower);
	}

	protected override void BaseUpdate()
	{
		base.BaseUpdate();
	}
}
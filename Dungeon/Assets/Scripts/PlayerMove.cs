using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : Character
{
	private GlobalInput playerInput;
	private float xInput;
	private float zInput;
	private bool jumpInput;

    void Start()
    {
		canMove = true;
		playerInput = gameObject.GetComponent<GlobalInput>();
		characterBody = gameObject.GetComponent<Rigidbody>();
		characterCollider = gameObject.GetComponent<CapsuleCollider>();
	}

    void Update()
	{
		inputs();

		move();

		jump();

		gravity();

		gameObject.transform.Translate(velocity * Time.deltaTime);

		collisionCase();


	}

	private void inputs()
	{
		xInput = playerInput.getXTiltMove() * acceleration;
		zInput = playerInput.getZTiltMove() * acceleration;
		jumpInput = Input.GetKey(KeyCode.Space);
	}

	private void move()
	{
		if (!canMove)
		{
			return;
		}

		//convert input to acceleration

		xInput *= acceleration;
		zInput *= acceleration;

		if (!onGround)
		{
			xInput /= 2;
			zInput /= 2;
		}

		if (xInput != 0)
			velocity.x += xInput * Time.deltaTime;
		else
		{
			velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 18);
		}

		if (zInput != 0)
			velocity.z += zInput * Time.deltaTime;
		else
		{
			velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime * 18);
		}

		//limit to max speed

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			velocity.x = Mathf.Clamp(velocity.x, -moveSpeed * sprintPower, moveSpeed * sprintPower);
			velocity.z = Mathf.Clamp(velocity.z, -moveSpeed * sprintPower, moveSpeed * sprintPower);
		}
		else
		{
			velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
			velocity.z = Mathf.Clamp(velocity.z, -moveSpeed, moveSpeed);
		}
	}

	private void collisionCase()
	{
		//determines where the player is in reference to all colliders
		for (int i = 0; i < collisionList.Count; i++)
		{
			Collider c = collisionList[i];
			float yExtent = c.bounds.extents.y;
			float yEffective = transform.position.y - characterCollider.bounds.extents.y + reachFromBottom;
			if (yEffective - c.transform.position.y > yExtent)
				groundCollision(c);
			else
			{

			}
		}
	}

	private void staticCollision(Collider c)
	{

	}

	private void groundCollision(Collider c)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + Vector3.up * .01f, -transform.up, out hitInfo, 1.02f, LayerMask.GetMask("Static", "Interactable")))
		{
			onGround = true;
		}
		else
			onGround = false;

		if (onGround)
		{
			if (velocity.y < 0)
				velocity.y = 0;
			transform.position = hitInfo.point + Vector3.up;	//switch to something more context sensitive for cases such as stairs
		}
	}

	private void jump()
	{
		if (onGround && jumpInput)
			velocity.y += Time.deltaTime * jumpForce;
	}

	private void gravity()
	{
		if (!onGround)
			velocity.y -= Time.deltaTime * gravityForce;
	}

	public void lockMovement()
	{
		canMove = false;
	}

	public void unlockMovement()
	{
		canMove = true;
	}

	public void OnTriggerEnter(Collider c)
	{
		collisionList.Add(c);
	}

	public void OnTriggerExit(Collider c)
	{
		collisionList.Remove(c);
	}
}

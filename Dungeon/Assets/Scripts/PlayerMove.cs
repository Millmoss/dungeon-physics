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
	private bool jumping = false;
	private bool sprintInput;

    void Start()
	{
		canMove = true;
		playerInput = gameObject.GetComponent<GlobalInput>();
		characterBody = gameObject.GetComponent<Rigidbody>();
		characterCollider = gameObject.GetComponent<CapsuleCollider>();
		groundCollision = gameObject.GetComponentInChildren<CharacterGroundCollision>();
		staticCollision = gameObject.GetComponentInChildren<CharacterStaticCollision>();
		nonstaticCollision = gameObject.GetComponentInChildren<CharacterNonstaticCollision>();

		characterBody.freezeRotation = true;
	}

	void Update()
	{
		inputs();
	}

	void FixedUpdate()
	{
		stabilize();

		move();

		jump();

		gravity();

		gameObject.transform.Translate(velocity * Time.deltaTime);

		//print(velocity);

		groundCollisionsHandler();
	}

	void LateUpdate()
	{
		stabilize();
	}

	private void inputs()
	{
		xInput = playerInput.getXTiltMove() * acceleration;
		zInput = playerInput.getZTiltMove() * acceleration;
		sprintInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		
		if (!onGround)
			jumpInput = false;
		else if (Input.GetKeyDown(KeyCode.Space) && onGround)
			jumpInput = true;
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
			xInput /= 50;
			zInput /= 50;
		}

		if (velocity.magnitude > moveSpeed)
		{
			xInput /= 10;
			zInput /= 10;
		}

		if (xInput != 0)
		{
			velocity.x += xInput * Time.deltaTime;
		}
		else
		{
			velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 18);
		}

		if (zInput != 0)
		{
			velocity.z += zInput * Time.deltaTime;
		}
		else
		{
			velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime * 18);
		}

		//limit to max speed

		if (sprintInput)
		{
			velocity = Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), sprintSpeed) + new Vector3(0, velocity.y, 0);
		}
		else
		{
			velocity = Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), moveSpeed) + new Vector3(0, velocity.y, 0);
		}
	}

	private void groundCollisionsHandler()
	{
		onGround = groundCollision.onGround();

		if (onGround && !jumping)	//eventually switch to account for moving objects
		{
			float y = groundCollision.groundHeight();

			if (velocity.y < 0)
				velocity.y = 0;

			Vector3 upPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, y, transform.position.z) + Vector3.up, .5f);

			transform.position = upPosition;	//switch to something more context sensitive for cases such as stairs
		}
	}

	private void jump()
	{
		if (onGround && !jumping && jumpInput)
		{
			velocity.y += Time.deltaTime * jumpForce;
			jumping = true;
		}

		if (jumping && velocity.y <= 0)
		{
			jumping = false;
		}
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

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 12)
		{
			nonStaticCollision(c.transform.position, c.gameObject.GetComponent<NonStatic>());
		}
	}

	void OnCollisionStay(Collision c)
	{
		if (c.gameObject.layer == 12)
		{
			nonStaticCollision(c.transform.position, c.gameObject.GetComponent<NonStatic>());
		}
	}

	void nonStaticCollision(Vector3 p, NonStatic ns)
	{
		Vector3 dir = p - transform.position;
		dir = new Vector3(dir.x, dir.y / 10, dir.z).normalized;
		Vector3 force = velocity * mass;
		ns.addForce(Mathf.Abs(Vector3.Dot(force, dir)) * dir);
	}
}

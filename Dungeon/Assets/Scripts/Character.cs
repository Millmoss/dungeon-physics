using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float acceleration = 3f;
	public float moveSpeed = 4.5f;
	public float sprintSpeed = 8.5f;
	public float jumpForce = 100f;
	public float gravityForce = 10f;
	public float mass = 70f;
	public float height = 1.7f;					//effective height of character for gameplay purposes
	public float stepHeight = .3f;
	protected Vector3 velocity = Vector3.zero;
	protected Rigidbody characterBody;
	protected Collider characterCollider;
	protected bool canMove;
	protected bool onGround;
	protected CharacterGroundCollision groundCollision;
	protected CharacterStaticCollision staticCollision;
	protected CharacterNonstaticCollision nonstaticCollision;

	void Start ()
	{

	}
	
	void Update ()
	{
		
	}

	protected void sanityCheck()			//must be called upon alteration of usually static values
	{
		if (stepHeight < 0)
			stepHeight = 0;
		else if (stepHeight > height)
			stepHeight = height;

		if (mass < 0)
			mass = 0;

		if (jumpForce < 0)
			jumpForce = 0;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float acceleration = 3f;
	public float moveSpeed = .8f;
	public float sprintPower = 1.7f;
	public float jumpForce = 100f;
	public float gravityForce = 10f;
	public float mass = 70f;
	public float height = 1.7f;					//effective height of character for gameplay purposes
	public float reachFromBottom = 1f;			//reach from bottom of character, meant for use with ledge grabbing/floor collisions
	protected Vector3 velocity = Vector3.zero;
	protected Rigidbody characterBody;
	protected Collider characterCollider;
	protected bool canMove;
	protected bool onGround;
	protected List<Collider> collisionList;

	void Start ()
	{
		collisionList = new List<Collider>();
	}
	
	void Update ()
	{
		
	}

	protected void sanityCheck()			//must be called upon alteration of usually static values
	{
		if (reachFromBottom < 0)
			reachFromBottom = 0;
		else if (reachFromBottom > height)
			reachFromBottom = height;

		if (mass < 0)
			mass = 0;

		if (jumpForce < 0)
			jumpForce = 0;
	}
}

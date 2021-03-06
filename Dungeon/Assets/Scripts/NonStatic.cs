﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStatic : MonoBehaviour
{
	[SerializeField] protected Vector3 velocity = Vector3.zero;
	[SerializeField] protected float gravityForce = 10f;
	[SerializeField] protected float mass = 5f;
	[SerializeField] [Range(0f, 1f)] protected float friction = .7f;
	[SerializeField] [Range(0f, 1f)] protected float airResistance = .1f;
	[SerializeField] [Range(0f, 1f)] protected float bounce = .1f;
	[SerializeField] [Range(-1f, 1f)] protected float adhesion = 0f;		//higher than zero to make more adhesive, lower than zero to make preventative of adhesion. Most objects should be zero
	protected Rigidbody selfBody;
	protected NonStaticSphere[] sphereColliders;
	protected NonStaticBox[] boxColliders;
	protected Vector3 force = Vector3.zero;
	protected Vector3 forceReference = Vector3.zero;
	protected float extent = 0;
	protected LayerMask collisionMask;
	protected float groundFrictionCoefficients = 0;		//between 0 and 1

	void Start ()
	{
		selfBody = gameObject.GetComponent<Rigidbody>();

		sphereColliders = GetComponentsInChildren<NonStaticSphere>();
		boxColliders = GetComponentsInChildren<NonStaticBox>();

		collisionMask = (LayerMask)111 << 9;
	}
	
	void Update ()
	{
		
	}

	void FixedUpdate()
	{
		stabilize();

		force = force - forceReference;

		force += gravityForce * mass * Vector3.down;

		handleColliders();

		print(force + ", " + velocity);

		velocityHandler();

		frictionHandler();

		transform.position += velocity * Time.deltaTime;

		forceReference = force;
	}

	void LateUpdate()
	{

	}

	protected void stabilize()
	{
		selfBody.velocity = Vector3.zero;
	}

	protected void handleColliders()
	{
		if (sphereColliders.Length > 0)
		{
			for (int i = 0; i < sphereColliders.Length; i++)
				sphereColliders[i].collisionsHandler();
		}
		if (boxColliders.Length > 0)
		{
			//for (int i = 0; i < boxColliders.Length; i++)
				//boxColliders[i].collisionsHandler();
		}
	}

	protected void generalCollisionHandler()
	{
		return;
		/*
		if (collisionList.Count == 0)
			return;

		if (selfColliderType == typeof(SphereCollider))
		{
			Vector3 groundingNormal = Vector3.zero;

			for (int i = 0; i < collisionList.Count; i++)
			{
				Collider c = collisionList[i].collider;

				Vector3 normal = (transform.position - c.ClosestPointOnBounds(transform.position)).normalized;

				if (Vector3.Angle(normal, Vector3.up) < 90)
					groundingNormal += normal;

				//get force from vector using closest point on object
				//if close enough to a grounding force, treat as though it is the ground
				//use velocity in moment and the friction of the two objects to determine rotation force

				if (c.gameObject.layer == 11 || c.gameObject.layer == 12)
				{
					//send force to object
				}
			}

			if (Vector3.Angle(groundingNormal, Vector3.up) < 1 && groundingNormal != Vector3.zero)		//account for slopes!!!
			{
				force += gravityForce * mass * Vector3.up;
				if (velocity.y < .05f)
				{
					velocity += Vector3.up * -velocity.y * bounce + Vector3.up * -velocity.y;
				}
				else if(velocity.y < 0)
				{
					velocity += Vector3.up * -velocity.y;
				}
			}

			//if on ground and upward force greater than gravity is non-existant, gravity should not apply
		}

		if (selfColliderType == typeof(BoxCollider))
		{
			Vector3 groundNormal = Vector3.zero;

			for (int i = 0; i < collisionList.Count; i++)
			{
				Collider c = collisionList[i].collider;

				if (c.gameObject.layer == 10 || (c.gameObject.layer == 12 && c.gameObject.GetComponent<NonStatic>().velocity == Vector3.zero))
				{

					Vector3 p = c.ClosestPointOnBounds(transform.position);
					if (transform.position.y > p.y)     //account for walls better!!!
						groundNormal += transform.position - p;

					//use distance to figure out the friction
					
				}
				else if (c.gameObject.layer == 11 || c.gameObject.layer == 12)
				{
					//forces are being applied and such, send forces and expect forces later
				}
			}

			groundNormal = groundNormal.normalized;

			force += gravityForce * mass * groundNormal;	//switch to accounting for normal difference and then apply friction
			//account for the rotation the normal creates if the object is not already at that rotation
		}*/
	}

	protected void frictionHandler()
	{
		//really bad temp code here
		if (Mathf.Abs(velocity.y) < .05f)
		{
			velocity *= 1 - friction * Time.deltaTime * 5;
		}
	}

	protected void velocityHandler()
	{
		velocity += (force / mass) * Time.deltaTime;
	}

	public Vector3 getVelocity()
	{
		return velocity;
	}

	public float getMass()
	{
		return mass;
	}

	public float getBounce()
	{
		return bounce;
	}

	public void addForce(Vector3 f)
	{
		force += f;
	}

	public void addVelocity(Vector3 v)
	{
		velocity += v;
	}
}

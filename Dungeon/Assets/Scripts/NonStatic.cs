using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStatic : MonoBehaviour
{
	[SerializeField] protected Vector3 velocity = Vector3.zero;
	[SerializeField] protected float gravityForce = 10f;
	[SerializeField] protected float mass = 70f;
	[SerializeField] protected float friction = .7f;
	[SerializeField] protected float airResistance = .1f;
	[SerializeField] protected float bounce = .1f;
	protected Rigidbody selfBody;
	protected Collider selfCollider;
	private float positionDif = 0;
	protected List<Collider> collisionList;
	protected System.Type selfColliderType;
	protected Vector3 force = Vector3.zero;
	protected Vector3 forceReference = Vector3.zero;

	void Start ()
	{
		collisionList = new List<Collider>();
		selfBody = gameObject.GetComponent<Rigidbody>();
		selfCollider = gameObject.GetComponent<Collider>();
		selfColliderType = selfCollider.GetType();
	}
	
	void Update ()
	{
		
	}

	void FixedUpdate()
	{
		stabilize();

		force = force - forceReference;

		force += gravityForce * Vector3.down;

		generalCollisionHandler();

		velocityHandler();

		forceReference = force;
	}

	void LateUpdate()
	{

	}

	protected void stabilize()
	{
		selfBody.velocity = Vector3.zero;
	}

	protected void generalCollisionHandler()
	{
		if (selfColliderType == typeof(SphereCollider))
		{
			bool onGround = false;

			for (int i = 0; i < collisionList.Count; i++)
			{
				Collider c = collisionList[i];

				//get force from vector using closest point on object
				//if close enough to a grounding force, treat as though it is the ground
				//use velocity in moment and the friction of the two objects to determine rotation force

				if (c.gameObject.layer == 11 || c.gameObject.layer == 12)
				{
					//send force to object
				}
			}

			//if on ground and upward force greater than gravity is non-existant, gravity should not apply
		}
		if (selfColliderType == typeof(BoxCollider))
		{
			float percentOnGround = 0;

			for (int i = 0; i < collisionList.Count; i++)
			{
				Collider c = collisionList[i];

				//get force from closest point against closest point to find rotation force
				//if close enough to a grounding force, treat as though it is the ground

				if (c.gameObject.layer == 11 || c.gameObject.layer == 12)
				{
					//send force to object
				}
			}

			//use percent on ground to figure out friction, whether gravity should even apply, and so on
		}
	}

	protected void velocityHandler()
	{

	}

	public void addForce(Vector3 f)
	{
		force += f;
	}

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 10 || c.gameObject.layer == 11 || c.gameObject.layer == 12)
			collisionList.Add(c.collider);
	}

	void OnCollisionExit(Collision c)
	{
		if (c.gameObject.layer == 10 || c.gameObject.layer == 11 || c.gameObject.layer == 12)
			collisionList.Remove(c.collider);
	}
}

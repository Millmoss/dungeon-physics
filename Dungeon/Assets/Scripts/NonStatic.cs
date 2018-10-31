using System.Collections;
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
	protected Collider selfCollider;
	protected List<Collider> collisionList;
	protected System.Type selfColliderType;
	protected Vector3 force = Vector3.zero;
	protected Vector3 forceReference = Vector3.zero;
	protected float extent = 0;
	protected LayerMask collisionMask;

	void Start ()
	{
		collisionList = new List<Collider>();
		selfBody = gameObject.GetComponent<Rigidbody>();
		selfBody.freezeRotation = true;
		selfCollider = gameObject.GetComponent<Collider>();
		selfColliderType = selfCollider.GetType();

		extent = Mathf.Sqrt(Mathf.Max(selfCollider.bounds.extents.x, selfCollider.bounds.extents.y, selfCollider.bounds.extents.z)
			+ Mathf.Max(selfCollider.bounds.extents.x, selfCollider.bounds.extents.y, selfCollider.bounds.extents.z));

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

		generalCollisionHandler();

		velocityHandler();

		print(velocity);

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

	protected void generalCollisionHandler()
	{
		if (collisionList.Count == 0)
			return;

		if (selfColliderType == typeof(SphereCollider))
		{
			Vector3 groundingNormal = Vector3.zero;

			for (int i = 0; i < collisionList.Count; i++)
			{
				Collider c = collisionList[i];

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

			if (Vector3.Angle(groundingNormal, Vector3.up) < 1 && groundingNormal != Vector3.zero)
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
			float percentOnGround = 0;

			for (int i = 0; i < collisionList.Count; i++)
			{
				Collider c = collisionList[i];

				RaycastHit hit = new RaycastHit();
				bool ray = Physics.Raycast(transform.position, Vector3.down, extent, 

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
		velocity += (force / mass) * Time.deltaTime;
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

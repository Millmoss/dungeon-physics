using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStatic : MonoBehaviour
{
	[SerializeField] protected Vector3 velocity = Vector3.zero;
	[SerializeField] protected float gravityForce = 10f;
	[SerializeField] protected float mass = 70f;
	[SerializeField] protected float friction = .5f;
	[SerializeField] protected float airResistance = .5f;
	protected Rigidbody selfBody;
	private float positionDif = 0;
	protected List<Collider> collisionList;

	void Start ()
	{
		collisionList = new List<Collider>();
		selfBody = gameObject.GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		print(velocity);
	}

	void FixedUpdate()
	{
		stabilize();

		//do some fancy maths to figure out how solidly the object is on the ground

		velocity += gravityForce * Vector3.down * Time.deltaTime;

		transform.position += velocity * Time.deltaTime;
	}

	void LateUpdate()
	{

	}

	protected void stabilize()
	{
		selfBody.velocity = Vector3.zero;
	}

	public void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 10)
			collisionList.Add(c.collider);
	}

	public void OnCollisionExit(Collision c)
	{
		if (c.gameObject.layer == 10)
			collisionList.Remove(c.collider);
	}
}

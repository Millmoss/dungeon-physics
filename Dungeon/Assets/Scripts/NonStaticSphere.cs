using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticSphere : MonoBehaviour
{
	private NonStatic nonStaticScript;
	private List<Collider> collisionList;

	void Start ()
	{
		nonStaticScript = GetComponentInParent<NonStatic>();
		collisionList = new List<Collider>();
	}
	
	void Update ()
	{
		
	}

	void FixedUpdate()
	{

	}

	public void collisionsHandler()
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

		if (Vector3.Angle(groundingNormal, Vector3.up) < 1 && groundingNormal != Vector3.zero)      //account for slopes!!!
		{
			nonStaticScript.addForce(10 * nonStaticScript.getMass() * Vector3.up);
			if (nonStaticScript.getVelocity().y < .05f)
			{
				nonStaticScript.addVelocity(Vector3.up * -nonStaticScript.getVelocity().y * nonStaticScript.getBounce() + Vector3.up * -nonStaticScript.getVelocity().y);
			}
			else if (nonStaticScript.getVelocity().y < 0)
			{
				nonStaticScript.addVelocity(Vector3.up * -nonStaticScript.getVelocity().y);
			}
		}

		//if on ground and upward force greater than gravity is non-existant, gravity should not apply
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 10 || c.gameObject.layer == 11 || c.gameObject.layer == 12)
			collisionList.Add(c);
	}

	void OnTriggerExit(Collider c)
	{
		if (c.gameObject.layer == 10 || c.gameObject.layer == 11 || c.gameObject.layer == 12)
			collisionList.Remove(c);
	}
}

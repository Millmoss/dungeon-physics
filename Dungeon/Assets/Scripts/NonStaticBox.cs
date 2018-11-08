using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticBox : MonoBehaviour
{
	private NonStatic nonStaticScript;
	private List<Collider> collisionList;

	void Start()
	{
		nonStaticScript = GetComponentInParent<NonStatic>();
		collisionList = new List<Collider>();
	}

	void Update()
	{

	}

	void FixedUpdate()
	{

	}

	public void collisionsHandler()
	{
		Vector3 groundNormal = Vector3.zero;

		for (int i = 0; i < collisionList.Count; i++)
		{
			Collider c = collisionList[i];

			if (c.gameObject.layer == 10 || (c.gameObject.layer == 12 && c.gameObject.GetComponent<NonStatic>().getVelocity() == Vector3.zero))
			{

				//CHECK ANGLE BETWEEN EACH SIDES NORMAL AND COLLISION'S NORMAL, IF THERE'S A SIMILAR NORMAL, IS FLAT ON SURFACE

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

		//force += gravityForce * mass * groundNormal;    //switch to accounting for normal difference and then apply friction
														//account for the rotation the normal creates if the object is not already at that rotation
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

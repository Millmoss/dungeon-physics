using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : MonoBehaviour
{
	private List<Collider> collisionList;
	private float selfExtent;
	private Collider groundCollider = null;
	private int groundIndex;

	void Start ()
	{
		collisionList = new List<Collider>();
		selfExtent = gameObject.GetComponent<SphereCollider>().bounds.extents.y;
	}
	
	void Update ()
	{

	}

	public bool onGround()
	{
		groundIndex = -1;

		if (collisionList.Count == 0)
		{
			return false;
		}

		for (int i = 0; i < collisionList.Count; i++)
		{
			Collider c = collisionList[i];
			float cExtent = c.transform.position.y + c.bounds.extents.y;
			if (cExtent >= transform.position.y && cExtent <= transform.position.y + selfExtent)
			{
				if (groundIndex == -1)
				{
					groundIndex = i;
				}
				else
				{
					Collider g = collisionList[groundIndex];
					float gExtent = g.transform.position.y + g.bounds.extents.y;
					if (gExtent < cExtent)
						groundIndex = i;
				}
			}
		}

		if (groundIndex != -1)
		{
			groundCollider = collisionList[groundIndex];
			return true;
		}

		return false;
	}

	public float groundHeight()		//MUST ALWAYS call onGround first
	{
		if (collisionList.Count == 0)
			return 0;
		
		float yExtent = groundCollider.transform.position.y + groundCollider.bounds.extents.y;
		return yExtent;
	}

	public bool stable()
	{
		if (collisionList.Count == 0 || groundCollider == null)
			return false;

		float yExtent = groundCollider.transform.position.y + groundCollider.bounds.extents.y;
		return Mathf.Abs(yExtent - transform.position.y) < .1f;
	}

	public void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 10 || c.gameObject.layer == 12)
			collisionList.Add(c);
	}

	public void OnTriggerExit(Collider c)
	{
		if (c.gameObject.layer == 10 || c.gameObject.layer == 12)
			collisionList.Remove(c);
	}
}

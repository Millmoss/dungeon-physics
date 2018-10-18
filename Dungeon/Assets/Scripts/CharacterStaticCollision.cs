using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStaticCollision : MonoBehaviour
{
	private List<Collider> collisionList;
	private float selfExtent;

	void Start()
	{
		collisionList = new List<Collider>();
		selfExtent = gameObject.GetComponent<CapsuleCollider>().radius;
	}

	public bool inStatic()
	{
		return false;
	}

	public Vector3 directionFromStatic(Vector3 velocity)
	{
		return Vector3.zero;
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 10)
			collisionList.Add(c);
	}

	void OnTriggerExit(Collider c)
	{
		if (c.gameObject.layer == 10)
			collisionList.Remove(c);
	}
}

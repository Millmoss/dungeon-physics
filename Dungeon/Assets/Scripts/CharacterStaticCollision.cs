using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStaticCollision : MonoBehaviour
{
	private CapsuleCollider selfCollider;
	private List<Collider> collisionList;
	private float selfExtentRadial;
	private float selfExtentY;

	void Start()
	{
		selfCollider = gameObject.GetComponent<CapsuleCollider>();
		collisionList = new List<Collider>();
		selfExtentRadial = selfCollider.radius;
		selfExtentY = selfCollider.bounds.extents.y - selfExtentRadial;
	}

	public bool inStatic()
	{
		if (collisionList.Count == 0)
			return false;

		for (int i = 0; i < collisionList.Count; i++)
		{
			Collider c = collisionList[i];

			if (Mathf.Abs(transform.position.y - c.transform.position.y) > selfExtentY + c.bounds.extents.y)
				return false;
			else
				return true;
		}

		return true;
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

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

	public Vector3 positionOnStatic(Vector3 velocity)
	{
		if (collisionList.Count == 0)
			return transform.position;

		Vector3 positionAverage = Vector3.zero;
		Vector3 directionAverage = Vector3.zero;
		int validCollisions = 0;

		for (int i = 0; i < collisionList.Count; i++)
		{
			Collider c = collisionList[i];

			if (Mathf.Abs(transform.position.y - c.transform.position.y) > selfExtentY + c.bounds.extents.y)
				continue;

			validCollisions++;

			Vector3 closestPoint = c.ClosestPointOnBounds(transform.position);
			Vector3 directionFromPoint = transform.position - closestPoint;
			directionFromPoint.y = 0;

			directionAverage += directionFromPoint.normalized * selfExtentRadial;
			positionAverage += closestPoint;
		}

		if (validCollisions == 0)
			return transform.position;

		positionAverage /= validCollisions;
		directionAverage /= validCollisions;

		return positionAverage + directionAverage;
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

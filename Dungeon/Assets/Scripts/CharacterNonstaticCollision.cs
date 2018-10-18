using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNonstaticCollision : MonoBehaviour
{
	private List<Collider> collisionList;
	private float selfExtent;

	void Start()
	{
		collisionList = new List<Collider>();
		selfExtent = gameObject.GetComponent<CapsuleCollider>().radius;
	}

	public void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 11 || c.gameObject.layer == 12)
			collisionList.Add(c);
	}

	public void OnTriggerExit(Collider c)
	{
		if (c.gameObject.layer == 11 || c.gameObject.layer == 12)
			collisionList.Remove(c);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	private Rigidbody swordBody;
	private Transform cameraTransform;
	private Item itemGeneric;
	private bool active = false;
	private bool leftAttack = false;
	private bool rightAttack = false;
	private float attackDirection = 0;
	private GameObject hand = null;
	private Vector3 handPosition;
	private string dominant = "undefined";
	private float cutTime = .6f;
	private float stabTime = .4f;
	private float attackTime = 0f;

	void Start ()
	{
		itemGeneric = GetComponent<Item>();
		itemGeneric.setType("Weapon");
		swordBody = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		if (!active)
		{
			active = itemGeneric.getActive();
			if (active)
			{
				hand = itemGeneric.getHand();
				handPosition = hand.transform.localPosition;
				cameraTransform = hand.GetComponentInParent<Camera>().transform;
				dominant = itemGeneric.getDominant();
			}
		}
		else
		{
			active = itemGeneric.getActive();
			if (!active)
			{
				hand = null;
				dominant = "undefined";
				leftAttack = false;
				rightAttack = false;
			}
		}
		if (active)
		{
			if (!rightAttack)
				leftAttack = itemGeneric.getLeft();
			if (!leftAttack)
				rightAttack = itemGeneric.getRight();
		}
	}

	void FixedUpdate()
	{
		if (active)
		{
			if (leftAttack)
			{
				if (dominant == "Left")
				{   //swing
					print("LEFT SWING");
				}
				else
				{   //stab
					print("LEFT STAB");
				}
			}
			else if (rightAttack)
			{
				if (dominant == "Right")
				{   //swing
					print("RIGHT SWING");
				}
				else
				{   //stab
					print("RIGHT STAB");
				}
			}
			else
			{
				hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, .5f);
				swordBody.rotation = Quaternion.Slerp(swordBody.rotation, cameraTransform.rotation, .45f);
			}
		}
	}
}

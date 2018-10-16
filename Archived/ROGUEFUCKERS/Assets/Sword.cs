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
	private ConfigurableJoint handJoint = null;

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
				//hand.transform.rotation = swordBody.rotation;
				handJoint = hand.GetComponent<ConfigurableJoint>();
				/*handJoint.angularXMotion = ConfigurableJointMotion.Locked;
				handJoint.angularYMotion = ConfigurableJointMotion.Locked;
				handJoint.angularZMotion = ConfigurableJointMotion.Locked;*/
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
				handJoint = null;
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
				handJoint.angularXMotion = ConfigurableJointMotion.Free;
				handJoint.angularYMotion = ConfigurableJointMotion.Free;
				handJoint.angularZMotion = ConfigurableJointMotion.Free;
				if (dominant == "Left")
				{   //swing
					print("LEFT SWING");
					itemGeneric.setAttackDamage(5);
				}
				else
				{   //stab
					print("LEFT STAB");
					itemGeneric.setAttackDamage(3);
				}
			}
			else if (rightAttack)
			{
				handJoint.angularXMotion = ConfigurableJointMotion.Free;
				handJoint.angularYMotion = ConfigurableJointMotion.Free;
				handJoint.angularZMotion = ConfigurableJointMotion.Free;
				if (dominant == "Right")
				{   //swing
					print("RIGHT SWING");
					itemGeneric.setAttackDamage(5);
				}
				else
				{   //stab
					print("RIGHT STAB");
					itemGeneric.setAttackDamage(3);
				}
			}
			else
			{
				itemGeneric.setAttackDamage(0);
				hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, .5f);
				swordBody.rotation = Quaternion.Slerp(swordBody.rotation, cameraTransform.rotation, 20 / Quaternion.Angle(swordBody.rotation, cameraTransform.rotation));
				if (Mathf.Abs(swordBody.rotation.eulerAngles.x - cameraTransform.rotation.eulerAngles.x) < .1f &&
					Mathf.Abs(swordBody.rotation.eulerAngles.y - cameraTransform.rotation.eulerAngles.y) < .1f &&
					Mathf.Abs(swordBody.rotation.eulerAngles.z - cameraTransform.rotation.eulerAngles.z) < .1f)
				{
					handJoint.angularXMotion = ConfigurableJointMotion.Locked;
					handJoint.angularYMotion = ConfigurableJointMotion.Locked;
					handJoint.angularZMotion = ConfigurableJointMotion.Locked;
				}
				//hand.transform.rotation = Quaternion.Slerp(hand.transform.rotation, cameraTransform.rotation, 20 / Quaternion.Angle(hand.transform.rotation, cameraTransform.rotation));
			}
		}
	}
}

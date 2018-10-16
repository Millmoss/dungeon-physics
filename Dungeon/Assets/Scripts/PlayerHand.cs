using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerHand : MonoBehaviour
{
	const float INTERACT_DISTANCE = 3f;

	public int handNum; //1 for right and 0 for left
	public Transform cameraTransform;
	public GameObject held = null;
	public GameObject hand;
	public Vector3 handPosition;
	private float throwTime = 0;
	private bool constrained = false;
	private Rigidbody heldBody;
	public float objectSpeed;
	public float throwForce = 50f;
	public Collider playerCollider;

	public PlayerHand otherHand;

	private Item itemScript;

	// Use this for initialization
	void Start()
	{
		handPosition = hand.transform.localPosition;
	}

	void Update()
	{
		//pickup of objects in left and right hand
		if ((Input.GetButtonDown("Left Grab") && handNum == 0) || (Input.GetButtonDown("Right Grab") && handNum == 1) && held == null)   //left trigger on controller, Q on keyboard
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask("Interactable")))
			{
				held = hitInfo.collider.transform.root.gameObject;
				throwTime = -1;
				heldBody = held.GetComponent<Rigidbody>();
				heldBody.useGravity = false;
				Collider[] cList = held.GetComponentsInParent<Collider>();
				for (int i = 0; i < cList.Length; i++)
					Physics.IgnoreCollision(gameObject.GetComponentInParent<Collider>(), cList[i]);
				itemScript = held.GetComponent<Item>();
				//itemScript.setInfo(GetComponent<CInfo>(), hand, "Right");
			}
		}
		else if ((Input.GetButton("Left Grab") && handNum == 0) || (Input.GetButton("Right Grab") && held != null && throwTime >= 0))
		{
			throwTime += Time.deltaTime;
		}
		else if ((Input.GetButtonUp("Left Grab") && handNum == 0) || (Input.GetButtonUp("Right Grab")))
		{
			if (throwTime > 0)
			{
				if (constrained)
					hand.GetComponent<ConfigurableJoint>().connectedBody = null;
				constrained = false;
				heldBody.useGravity = true;
				Collider[] cList = held.GetComponentsInParent<Collider>();
				for (int i = 0; i < cList.Length; i++)
					Physics.IgnoreCollision(gameObject.GetComponentInParent<Collider>(), cList[i], false);

				heldBody.AddForce(cameraTransform.forward * Mathf.Clamp(throwTime, 0, 1) * throwForce);

				heldBody = null;
				held = null;
				itemScript.resetInfo();
				itemScript = null;
				hand.transform.localPosition = handPosition;
				throwTime = 0;
			}
			else
				throwTime = 0;
		}
		
		if (itemScript != null)
		{
			if (handNum == 0)
			{
				itemScript.setLeft(Input.GetMouseButton(0));
				if (!otherHand.holding())
				{
					itemScript.setRight(Input.GetMouseButton(1));
				}
			}
			else
			{
				itemScript.setRight(Input.GetMouseButton(1));
				if (!otherHand.holding())
				{
					itemScript.setLeft(Input.GetMouseButton(0));
				}
			}
		}
	}

    void FixedUpdate()
	{
		if (held != null)
		{
			float dist = Vector3.Distance(heldBody.transform.position, hand.transform.position);
			if (dist > .05f)
				heldBody.velocity = (hand.transform.position - heldBody.transform.position).normalized * objectSpeed * Time.deltaTime * dist;//extend
			else if (!constrained)
			{
				hand.GetComponent<ConfigurableJoint>().connectedBody = heldBody;
				constrained = true;
			}
			if (constrained)
			{
				
			}
		}
	}

	public bool holding()
	{
		if (held == null)
			return false;
		return true;
	}
}
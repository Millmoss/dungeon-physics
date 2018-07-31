using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteract : MonoBehaviour
{
	const float INTERACT_DISTANCE = 3f;
	
	public Transform cameraTransform;
	public GameObject heldRight = null;
	public GameObject heldLeft = null;
	public GameObject leftHand;
	public Vector3 leftHandPosition;
	public GameObject rightHand;
	public Vector3 rightHandPosition;
	private float leftThrow = 0;
	private float rightThrow = 0;
	private bool leftConstrained = false;
	private bool rightConstrained = false;
	private Rigidbody heldLeftBody;
	private Rigidbody heldRightBody;
	public float objectSpeed;
	public float swordForce = 5f;
	public float throwForce = 50f;
	public Collider playerCollider;

	private Item itemScriptLeft;
	private Item itemScriptRight;

	private PlayerCam playerCamScript;

	// Use this for initialization
	void Start()
	{
		cameraTransform = gameObject.GetComponentInChildren<Camera>().gameObject.transform;
		playerCamScript = gameObject.GetComponent<PlayerCam>();
		leftHandPosition = leftHand.transform.localPosition;
		rightHandPosition = rightHand.transform.localPosition;
	}

	void Update()
	{
		//pickup of objects in left and right hand
		if (Input.GetButtonDown("Left Grab") && heldLeft == null)   //left trigger on controller, Q on keyboard
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask("Interactable")))
			{
				heldLeft = hitInfo.collider.transform.root.gameObject;
				leftThrow = -1;
				heldLeftBody = heldLeft.GetComponent<Rigidbody>();
				heldLeftBody.useGravity = false;
				Collider[] cList = heldLeft.GetComponentsInChildren<Collider>();
				for (int i = 0; i < cList.Length; i++)
					Physics.IgnoreCollision(gameObject.GetComponentInChildren<Collider>(), cList[i]);
				itemScriptLeft = heldLeft.GetComponent<Item>();
				itemScriptLeft.setInfo(GetComponent<CInfo>(), leftHand, "Right");
			}
		}
		else if (Input.GetButton("Left Grab") && heldLeft != null && leftThrow >= 0)
		{
			leftThrow += Time.deltaTime;
		}
		else if (Input.GetButtonUp("Left Grab"))
		{
			if (leftThrow > 0)
			{
				if (leftConstrained)
					leftHand.GetComponent<ConfigurableJoint>().connectedBody = null;
				leftConstrained = false;
				heldLeftBody.useGravity = true;
				Collider[] cList = heldLeft.GetComponentsInChildren<Collider>();
				for (int i = 0; i < cList.Length; i++)
					Physics.IgnoreCollision(gameObject.GetComponentInChildren<Collider>(), cList[i], false);

				heldLeftBody.AddForce(cameraTransform.forward * Mathf.Clamp(leftThrow, 0, 1) * throwForce);

				heldLeftBody = null;
				heldLeft = null;
				itemScriptLeft.resetInfo();
				itemScriptLeft = null;
				leftHand.transform.localPosition = leftHandPosition;
				leftThrow = 0;
			}
			else
				leftThrow = 0;
		}

		if (Input.GetButtonDown("Right Grab") && heldRight == null)   //left trigger on controller, Q on keyboard
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, INTERACT_DISTANCE, LayerMask.GetMask("Interactable")))
			{
				heldRight = hitInfo.collider.transform.root.gameObject;
				rightThrow = -1;
				heldRightBody = heldRight.GetComponent<Rigidbody>();
				heldRightBody.useGravity = false;
				Collider[] cList = heldRight.GetComponentsInChildren<Collider>();
				for (int i = 0; i < cList.Length; i++)
					Physics.IgnoreCollision(gameObject.GetComponentInChildren<Collider>(), cList[i]);
				itemScriptRight = heldRight.GetComponent<Item>();
				itemScriptRight.setInfo(GetComponent<CInfo>(), rightHand, "Right");
			}
		}
		else if (Input.GetButton("Right Grab") && heldRight != null && rightThrow >= 0)
		{
			rightThrow += Time.deltaTime;
		}
		else if (Input.GetButtonUp("Right Grab"))
		{
			if (rightThrow > 0)
			{
				if (rightConstrained)
					rightHand.GetComponent<ConfigurableJoint>().connectedBody = null;
				rightConstrained = false;
				heldRightBody.useGravity = true;
				Collider[] cList = heldRight.GetComponentsInChildren<Collider>();
				for (int i = 0; i < cList.Length; i++)
					Physics.IgnoreCollision(gameObject.GetComponentInChildren<Collider>(), cList[i], false);

				heldRightBody.AddForce(cameraTransform.forward * Mathf.Clamp(rightThrow, 0, 1) * throwForce);

				heldRightBody = null;
				heldRight = null;
				itemScriptRight.resetInfo();
				itemScriptRight = null;
				rightHand.transform.localPosition = rightHandPosition;
				rightThrow = 0;
			}
			else
				rightThrow = 0;
		}

		if (itemScriptLeft != null)
		{
			itemScriptLeft.setLeft(Input.GetMouseButton(0));
			if (itemScriptRight == null)
			{
				itemScriptLeft.setRight(Input.GetMouseButton(1));
			}
		}
		if (itemScriptRight != null)
		{
			itemScriptRight.setRight(Input.GetMouseButton(1));
			if (itemScriptLeft == null)
			{
				itemScriptRight.setLeft(Input.GetMouseButton(0));
			}
		}
	}

    void FixedUpdate()
	{
		if (heldLeft != null)
		{
			float dist = Vector3.Distance(heldLeftBody.transform.position, leftHand.transform.position);
			if (dist > .1f)
				heldLeftBody.velocity = (leftHand.transform.position - heldLeftBody.transform.position).normalized * objectSpeed * Time.deltaTime * dist;
			else if (!leftConstrained)
			{
				leftHand.GetComponent<ConfigurableJoint>().connectedBody = heldLeftBody;
				leftConstrained = true;
			}
			if (leftConstrained)
			{
				//heldLeft.transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
			}
		}

		if (heldRight != null)
		{
			float dist = Vector3.Distance(heldRightBody.transform.position, rightHand.transform.position);
			if (dist > .1f)
				heldRightBody.velocity = (rightHand.transform.position - heldRightBody.transform.position).normalized * objectSpeed * Time.deltaTime * dist;
			else if (!rightConstrained)
			{
				rightHand.GetComponent<ConfigurableJoint>().connectedBody = heldRightBody;
				rightConstrained = true;
			}
			if (rightConstrained)
			{
				//heldRight.transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
			}
		}
	}
}
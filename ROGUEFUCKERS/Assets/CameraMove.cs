using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraMove : MonoBehaviour
{
	public GlobalInput inputScript;
	public Camera cam;
	public Transform camPoint;
	private Vector3 playerChange;
	private float xTiltLook;
	private float zTiltLook;
	private float rotateSpeed;
	public bool mouseLock = true;
	private bool rotateCamera = true;
	private float zRotationDif = 0;

	void Start()
	{
		transform.position = new Vector3(camPoint.transform.position.x, camPoint.transform.position.y, camPoint.transform.position.z - 3f);
		playerChange = camPoint.transform.position;
		rotateSpeed = 160f;
		transform.RotateAround(camPoint.transform.position, Vector3.up, 180);
		zRotationDif = transform.rotation.eulerAngles.y - zRotationDif;
		camPoint.transform.Rotate(Vector3.up, zRotationDif);
		zRotationDif = transform.rotation.eulerAngles.y;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.LeftAlt))
		{
			mouseLock = !mouseLock;
		}

		//camera control input
		xTiltLook = inputScript.getXTiltLook();
		zTiltLook = 0 - inputScript.getZTiltLook();

		if (mouseLock)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			rotateCamera = true;
		}
		else
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			rotateCamera = false;
		}

		cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0.0f);
		camPoint.LookAt(Camera.main.transform);
		camPoint.transform.eulerAngles = new Vector3(0.0f, camPoint.transform.eulerAngles.y, 0.0f);

	}

	void FixedUpdate()
	{
		if (rotateCamera)
		{
			//camera movement
			playerChange = camPoint.transform.position - playerChange;
			//playerChange.y = 0;
			transform.position += playerChange;

			//camera y rotation //IF ROTATION GETS WEIRD REORDER X AND Y!!!
			transform.RotateAround(camPoint.transform.position, Vector3.up, xTiltLook * Time.deltaTime * rotateSpeed);
			zRotationDif = transform.rotation.eulerAngles.y - zRotationDif;
			camPoint.transform.Rotate(Vector3.up, zRotationDif);
			zRotationDif = transform.rotation.eulerAngles.y;

			//camera x rotation //DO NOT REORDER X AND Y!!!
			float xRotation = cam.transform.rotation.eulerAngles.x;
			float futureRotation = zTiltLook * Time.deltaTime * rotateSpeed;
			cam.transform.RotateAround(camPoint.position, camPoint.right, futureRotation * -1f);
			if ((xRotation + futureRotation) >= 45 && (xRotation + futureRotation) <= 180)
			{
				cam.transform.RotateAround(camPoint.position, camPoint.right, futureRotation * 1f);
			}
			else if ((xRotation + futureRotation) <= 324 && (xRotation + futureRotation) > 180)
			{
				cam.transform.RotateAround(camPoint.position, camPoint.right, futureRotation * 1f);
			}

			//update player position
			playerChange = camPoint.position;
		}
	}

	void LateUpdate()
	{
		//movement stabilization
		playerChange = camPoint.position - playerChange;
		//playerChange.y = 0;
		transform.position += playerChange;
		playerChange = camPoint.position;
	}
}
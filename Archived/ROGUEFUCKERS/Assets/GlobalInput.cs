using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GlobalInput : MonoBehaviour
{
	public float getXTiltMove()
	{
		float tempx = CrossPlatformInputManager.GetAxis("Horizontal");
		float tempz = CrossPlatformInputManager.GetAxis("Vertical");
		Vector3 tempv = Vector3.ClampMagnitude(new Vector3(tempx, 0, tempz), 1);
		if (Mathf.Abs(tempv.x) >= .15f)
			return tempv.x;
		return 0;
	}

	public float getZTiltMove()
	{
		float tempx = CrossPlatformInputManager.GetAxis("Horizontal");
		float tempz = CrossPlatformInputManager.GetAxis("Vertical");
		Vector3 tempv = Vector3.ClampMagnitude(new Vector3(tempx, 0, tempz), 1);
		if (Mathf.Abs(tempv.z) >= .15f)
			return tempv.z;
		return 0;
	}

	public float getXTiltLook()
	{
		float temp = CrossPlatformInputManager.GetAxis("Mouse X");
		if (Mathf.Abs(temp) >= .07f)
			return temp;
		return 0;
	}

	public float getZTiltLook()
	{
		float temp = CrossPlatformInputManager.GetAxis("Mouse Y");
		if (Mathf.Abs(temp) >= .07f)
			return temp;
		return 0;
	}
}

using UnityEngine;

public class Player : MonoBehaviour
{
	/*public GlobalInput input;
	public Camera cam;
	public Rigidbody swordBody;
	public float moveSpeed = .5f;
	public float maxSpeed = .5f;
	public float swordForce = 5f;

	private float xMove = 0;
	private float zMove = 0;
	private float x = 0;
	private float z = 0;
	private bool mouse;
	private float swordTime = 0;

	void Start ()
	{
		input = GetComponent<GlobalInput>();
	}

	void Update()
	{
		x = input.getXTiltMove();
		z = input.getZTiltMove();
		mouse = Input.GetKey(KeyCode.Mouse0);
	}

	void FixedUpdate()
	{
		xMove += x * moveSpeed * Time.deltaTime;
		if (xMove > maxSpeed && x > 0)
			xMove = maxSpeed;
		else if (xMove < -maxSpeed && x < 0)
			xMove = -maxSpeed;
		else if (x == 0)
		{
			if (xMove > moveSpeed)
			{
				xMove -= moveSpeed * Time.deltaTime;
			}
			else if (xMove < moveSpeed)
			{
				xMove += moveSpeed * Time.deltaTime;
			}

			if (Mathf.Abs(xMove) < moveSpeed)
				xMove = 0;
		}

		zMove += z * moveSpeed * Time.deltaTime;
		if (zMove > maxSpeed && z > 0)
			zMove = maxSpeed * z;
		else if (zMove < -maxSpeed && z < 0)
			zMove = -maxSpeed;
		else if (z == 0)
		{
			if (zMove > moveSpeed)
			{
				zMove -= moveSpeed * Time.deltaTime;
			}
			else if (zMove < moveSpeed)
			{
				zMove += moveSpeed * Time.deltaTime;
			}

			if (Mathf.Abs(zMove) < moveSpeed)
				zMove = 0;
		}

		gameObject.transform.Translate(xMove, 0, zMove);
		gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(new Vector3(0, cam.transform.rotation.eulerAngles.y)), .1f);

		if (mouse)
		{
			if(swordTime < .4f)
				swordBody.AddForce(swordBody.transform.forward * swordForce * 8 * swordTime);
			else
				swordBody.AddForce(swordBody.transform.forward * swordForce / 2);
			swordTime += Time.deltaTime;
		}
		else
		{
			swordBody.AddForce(-swordBody.transform.forward * swordForce / 3 + swordBody.transform.up * swordForce / 3);
			swordTime = 0;
		}
	}*/
}

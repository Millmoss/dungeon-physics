using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour
{
    private float xMove;
    private float zMove;
    public float moveSpeed = 3f;
    public float maxSpeed = .3f;
	public float jumpForce = 100f;
	public GlobalInput playerInput;
	public Rigidbody playerBody;
	public Collider playerCollider;
	private float maxSpeedOriginal;		// CHANGE TO SPRINT MODDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
	private bool canMove;
	private bool onGround;

	//public AudioSource walk;

    void Start()
    {
		//walk = walk.GetComponent<AudioSource>();
        xMove = 0f;
        zMove = 0f;
		maxSpeedOriginal = maxSpeed;
		canMove = true;
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.LeftShift))
			maxSpeed *= 1.7f;
		else if (Input.GetKeyUp(KeyCode.LeftShift))
			maxSpeed = maxSpeedOriginal;

		if (!canMove)
		{
			return;
		}
        if (playerInput.getZTiltMove() > 0)
        {
			//walk.mute = false;
            zMove += moveSpeed * Time.deltaTime;
            if (zMove > maxSpeed)
                zMove = maxSpeed;
        }
        else if (zMove > 0 && Mathf.Abs(zMove) >= .05f)
        {
            zMove = Mathf.Lerp(zMove, zMove - (moveSpeed * Time.deltaTime), .5f);
            //walk.mute = true;
        }
        if (playerInput.getXTiltMove() > 0)
        {
			//walk.mute = false;
            xMove += moveSpeed * Time.deltaTime;
            if (xMove > maxSpeed)
                xMove = maxSpeed;
        }
        else if (xMove > 0 && Mathf.Abs(xMove) >= .05f)
        {
            xMove = Mathf.Lerp(xMove, xMove - (moveSpeed * Time.deltaTime), .5f);
            //walk.mute = true;
        }
        if (playerInput.getZTiltMove() < 0)
        {
			//walk.mute = false;
            zMove -= moveSpeed * Time.deltaTime;
            if (zMove < -maxSpeed)
                zMove = -maxSpeed;
        }
        else if (zMove < 0 && Mathf.Abs(zMove) >= .05f)
        {
            zMove = Mathf.Lerp(zMove, zMove + (moveSpeed * Time.deltaTime), .5f);
            //walk.mute = true;
        }
        if (playerInput.getXTiltMove() < 0)
        {
			//walk.mute = false;
            xMove -= moveSpeed * Time.deltaTime;
            if (xMove < -maxSpeed)
                xMove = -maxSpeed;
        }
        else if (xMove < 0 && Mathf.Abs(xMove) >= .05f)
        {
            xMove = Mathf.Lerp(xMove, xMove + (moveSpeed * Time.deltaTime), .5f);
			//walk.mute = true;
        }
        if (Mathf.Abs(xMove) < .05f)
        {
            xMove = Mathf.Lerp(xMove, 0f, .9f);
        }
        if (Mathf.Abs(zMove) < .05f)
        {
            zMove = Mathf.Lerp(zMove, 0f, .9f);
        }

		if (Input.GetButtonDown("Jump"))	//change to manual jumping outside of physics system
		{
			if (onGround)
			{
				playerBody.AddForce(new Vector3(0, jumpForce, 0));
			}
		}
    }

    void FixedUpdate()
    {
        gameObject.transform.Translate(xMove, 0, zMove);
		Vector3 slow = Vector3.Lerp(playerBody.velocity, Vector3.zero, .5f);
		playerBody.velocity = new Vector3(slow.x, playerBody.velocity.y, slow.z);

		if (playerCollider.transform.position.y < 1.01f)    //change to raycast or box collider on ground later on
		{
			onGround = true;
		}
		else
		{
			onGround = false;
		}
	}

	public void lockMovement() {
		canMove = false;
	}

	public void unlockMovement() {
		canMove = true;
	}
}
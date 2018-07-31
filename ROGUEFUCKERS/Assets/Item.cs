using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	private CInfo character = null;
	private GameObject hand = null;
	private bool left = false;
	private bool right = false;
	private string dominant = "undefined";
	private string type = "undefined";

	void Start ()
	{

	}
	
	void Update ()
	{

	}

	public void setInfo(CInfo c, GameObject h, string d)
	{
		character = c;
		hand = h;
		dominant = d;
	}

	public void resetInfo()
	{
		character = null;
		hand = null;
		dominant = "undefined";
	}

	public bool getActive()
	{
		return hand != null;
	}

	public GameObject getHand()
	{
		return hand;
	}

	public string getDominant()
	{
		return dominant;
	}

	public void setLeft(bool l)
	{
		left = l;
	}

	public void setRight(bool r)
	{
		right = r;
	}

	public bool getLeft()
	{
		return left;
	}

	public bool getRight()
	{
		return right;
	}

	public void setType(string t)
	{
		type = t;
	}

	public string getType()
	{
		return type;
	}
}

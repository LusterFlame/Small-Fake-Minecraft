using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class playerCtrl : MonoBehaviour
{
	//player need rigid body!
	private void OnCollisionEnter(Collision collision)
	{
		/*havn't done yet
		//maybe use tag or something...
		switch (collision.transform.name)
		{
			case blockname:
				onGround = true;
				Debug.Log("onGround");
				break;
			default:
				break;
		}
		*/
	}

	private void RightMouseClick()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit ray_cast_hit;
			if (Physics.Raycast(ray, out ray_cast_hit))
			{
				rightPlace = ray_cast_hit.point;
				Debug.Log("rightClick");
				rightClick = true;
			}
		}
	}

	private void tryDestoryBlock()
	{
		if (Input.GetMouseButtonDown(0) && canDestory)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rh;

			if (Physics.Raycast(ray, out rh))
			{
				Debug.Log("click: " + rh.collider.name);
				if (rh.collider.name == blockname)
				{
					Destroy(rh.collider.gameObject);

				}
			}
		}

	}

	private void headRotate()
	{

		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}

	/*return the vector3 that player face to (is relative) , then rotate it [angle] degree on Y-axis(change x & z)*/
	Vector3 playerDir(double angle)
	{
		double forward = Math.Sin(((double)yaw + angle) * Math.PI / 180);
		double left = Math.Cos(((double)yaw + angle) * Math.PI / 180);
		return new Vector3((float)forward, 0, (float)left);
	}

	private void moveControl()
	{
		//animator.SetFloat("speed", 0f); //animation switch

		if (Input.GetKey(KeyCode.W))
		{
			//transform.localPosition += moving_speed * Time.deltaTime * Vector3.forward;
			GetComponent<Rigidbody>().velocity = playerDir(0) * movingSpeed * Time.deltaTime;

			//animator.SetFloat("speed", moving_speed);
		}
		if (Input.GetKey(KeyCode.D))
		{
			GetComponent<Rigidbody>().velocity = playerDir(90) * movingSpeed * Time.deltaTime;

		}
		if (Input.GetKey(KeyCode.S))
		{
			GetComponent<Rigidbody>().velocity = playerDir(180) * movingSpeed * Time.deltaTime;

		}
		if (Input.GetKey(KeyCode.A))
		{
			GetComponent<Rigidbody>().velocity = playerDir(270) * movingSpeed * Time.deltaTime;

		}
		
	}

	private void jumpControl()
	{
		if (Input.GetKey(KeyCode.Space) && onGround)
		{
			GetComponent<Rigidbody>().AddForce(0, jumpForce * GetComponent<Rigidbody>().mass, 0);
			//transform.localPosition += jump_force * Time.deltaTime * Vector3.up;
			onGround = false;
		}
	}

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		movingSpeed = 0;
		jumpControl();
		moveControl();
		headRotate();
		tryDestoryBlock();

		RightMouseClick();
	}

	/*↓ maybe change in the future*/
	string blockname = "grass";//the name of gameobject of floor


	//public Vector3 moving_vector;
	private MeshRenderer meshRenderer;

	[SerializeField] float movingSpeed = 10f;
	public float jumpForce = 1000f;
	
	[SerializeField] bool rightClick = false;//true when player rightclick a block

	[SerializeField] bool onGround = false; //if player standing on ground
	[SerializeField] bool canDestory = true; //if player can destroy blocks

	Animator animator;

	///
	public float speedH = 2.0f;
	public float speedV = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;
	///
}

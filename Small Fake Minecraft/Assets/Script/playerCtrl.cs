using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class playerCtrl : MonoBehaviour
{
	//player need rigid body!

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.transform.name)
		{
			case "Grass Block(Clone)":
				onGround = true;
				Debug.Log("onGround");
				break;
			default:
				break;
		}
	}

	private void RightMouseClick()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit ray_cast_hit;
			if (Physics.Raycast(ray, out ray_cast_hit))
			{
				Debug.Log("rightClick");
				rightClick = true;
			}
		}
	}

	private void tryDestoryBlock()
	{
		if (Input.GetMouseButtonDown(0) && canDestory)
		{
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Ray ray = Camera.main.ViewportPointToRay(new Vector2((float)0.5, (float)0.5));
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
		if (lockMouse)
		{
			yaw += speedH * Input.GetAxis("Mouse X");
			pitch -= speedV * Input.GetAxis("Mouse Y");

			transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

		}
	}

	/*return the vector3 that player face to (is relative)*/
	Vector3 playerFront(int mode)//mode 0,1,2 is none,front,back
	{
		if (mode == 0)
		{
			return Vector3.zero;
		}
		double forward = Math.Sin(((double)yaw + (mode - 1) * 180) * Math.PI / 180);
		double left = Math.Cos(((double)yaw + (mode - 1) * 180) * Math.PI / 180);
		return new Vector3((float)forward, 0, (float)left);
	}
	Vector3 playerLeft(int mode)//mode 0,1,2 is none,left,right
	{
		if (mode == 0)
		{
			return Vector3.zero;
		}
		double forward = Math.Sin(((double)yaw + (mode - 1) * 180 - 90) * Math.PI / 180);
		double left = Math.Cos(((double)yaw + (mode - 1) * 180 - 90) * Math.PI / 180);
		return new Vector3((float)forward, 0, (float)left);
	}

	int playerGoFront = 0;
	int playerGoLeft = 0;
	private void playerWalk()
	{
		if (onGround)
		{
			GetComponent<Rigidbody>().velocity = (playerFront(playerGoFront) + playerLeft(playerGoLeft)) * movingSpeed * Time.deltaTime * movingSpeedRate;
		}
		else
		{
			//GetComponent<Rigidbody>().velocity = Vector3.Normalize(playerFront(playerGoFront) + playerLeft(playerGoLeft)) * movingSpeed * Time.deltaTime*movingSpeedRateInAir;
		}
	}

	private void moveControl()
	{

		//animator.SetFloat("speed", 0f); //animation switch
		playerGoFront = playerGoLeft = 0;
		if (Input.GetKey(KeyCode.W))
		{
			playerGoFront = 1;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			playerGoFront = 2;
		}

		if (Input.GetKey(KeyCode.A))
		{
			playerGoLeft = 1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			playerGoLeft = 2;
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			movingSpeedRate = 2;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			movingSpeedRate = 1;
		}
		playerWalk();

	}

	private void jumpControl()
	{
		if (Input.GetKey(KeyCode.Space) && onGround)
		{
			GetComponent<Rigidbody>().AddForce(0, jumpForce * GetComponent<Rigidbody>().mass, 0);
			onGround = false;
		}
	}

	GameObject pauseCanvasClone;
	private void mouseCtrl()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			lockMouse = !lockMouse;
			if (lockMouse)
			{
				Time.timeScale = 1f;
				Destroy(pauseCanvasClone);
			}
			else
			{
				Time.timeScale = 0f;
				pauseCanvasClone = Instantiate(pauseCanvas, Vector2.zero, Quaternion.identity);
			}
		}
	}

	public void playerESC()
	{
		lockMouse = true;
		Time.timeScale = 1f;
	}


	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		Cursor.lockState = CursorLockMode.Locked;//lock mouse
	}

	// Update is called once per frame
	void Update()
	{
		mouseCtrl();
		if (lockMouse)
		{
			Cursor.lockState = CursorLockMode.Locked;
			jumpControl();
			moveControl();
			headRotate();
			tryDestoryBlock();
			RightMouseClick();
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	/*↓ maybe change in the future*/
	string blockname = "Grass Block(Clone)";//the name of gameobject that player can break


	//public Vector3 moving_vector;
	private MeshRenderer meshRenderer;

	[SerializeField] float movingSpeed = 10f;
	[SerializeField] float movingSpeedRate = 1.0f;
	//[SerializeField] float movingSpeedRateInAir = 0.3f;
	public float jumpForce = 1000f;

	[SerializeField] bool rightClick = false;//true when player rightclick a block

	public bool onGround = false; //if player standing on ground
	[SerializeField] bool canDestory = true; //if player can destroy blocks

	Animator animator;

	///
	public float speedH = 2.0f;
	public float speedV = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;
	///

	[SerializeField] bool lockMouse = true;
	public GameObject pauseCanvas;//Pause Canvas
}

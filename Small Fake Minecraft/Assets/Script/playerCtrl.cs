using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class playerCtrl : MonoBehaviour
{
	//player need rigid body!
	private void OnTriggerStay(Collider other)
	{
		onGround = true;
	}

	private void OnTriggerExit(Collider other)
	{
		onGround = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
	}

	private void RightMouseClick()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit ray_cast_hit;


			//ignore "ignore RayCast" layer
			int raylayerMask = 1 << 2;
			raylayerMask = ~raylayerMask;
			if (Physics.Raycast(ray, out ray_cast_hit, 16f, raylayerMask))
			{
				Debug.Log("Rightclick: " + ray_cast_hit.collider.name);
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

			//ignore "ignore RayCast" layer
			int raylayerMask = 1 << 2;
			raylayerMask = ~raylayerMask;

			if (Physics.Raycast(ray, out rh, float.PositiveInfinity, raylayerMask))//2:ignore RayCast
			{
				foreach (string blockname in blockList)
				{
					if (rh.collider.name == blockname)//will be rh.collider.tag
					{
						Destroy(rh.collider.gameObject);
						break;
					}
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

			yaw %= 360;
			pitch %= 180;
		}
		transform.GetChild(0).eulerAngles = new Vector3(0, yaw, 0.0f);
		transform.GetChild(0).GetChild(0).eulerAngles = new Vector3(pitch, yaw, 0.0f);
		//transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetChild(0).transform.eulerAngles = new Vector3(51.563f + pitch, -82.956f + yaw, 8.315f + 0.0f);//playerHead

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

	Vector3 groundSpd;
	private void playerWalk()
	{
		if (onGround)
		{
			GetComponent<Rigidbody>().velocity = groundSpd = (playerFront(playerGoFront) + playerLeft(playerGoLeft)).normalized * movingSpeed * movingSpeedRate;
			Debug.Log(GetComponent<Rigidbody>().velocity.magnitude);
		}
		else
		{
			//maybe do something here?
		}
	}

	private void moveControl()
	{

		animator.SetBool("runForward", false);
		animator.SetBool("runBackward", false);
		playerGoFront = playerGoLeft = 0;
		if (Input.GetKey(KeyCode.W))
		{
			playerGoFront = 1;
			animator.SetBool("runForward", true);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			playerGoFront = 2;
			animator.SetBool("runBackward", true);
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
	GameObject hotbarCanvasClone;
	GameObject keyTCanvasClone;
	private bool keyT = false;
	private void mouseCtrl()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			lockMouse = !lockMouse;
			if (lockMouse)
			{
				if (keyT)
				{
					closeKeyTCanvas();
				}
				else
				{
					Time.timeScale = 1f;
					Destroy(pauseCanvasClone);
					hotbarCanvasClone = Instantiate(hotbarCanvas, Vector2.zero, Quaternion.identity);
				}
			}
			else
			{
				Time.timeScale = 0f;
				pauseCanvasClone = Instantiate(pauseCanvas, Vector2.zero, Quaternion.identity);
				Destroy(hotbarCanvasClone);
			}
		}

		if (Input.GetKey(KeyCode.T) && lockMouse)
		{
			lockMouse = false;
			keyT = true;
			keyTCanvasClone = Instantiate(keyTCanvas, Vector2.zero, Quaternion.identity);

		}
	}

	private void closeKeyTCanvas()
	{
		keyT = false;
		Destroy(keyTCanvasClone);
	}

	public void playerESC()
	{
		lockMouse = true;
		Time.timeScale = 1f;
	}

	private void commandInput()
	{
		if (keyT)
		{
			string cmd = inputField.GetComponent<Text>().text;//still can't get the command, keep trying
			Debug.Log(cmd);
		}
	}

	private void Awake()
	{
		/*related to GetChild*/
		inputField = keyTCanvas.gameObject.transform.GetChild(0).GetChild(1).gameObject;
		animator = transform.GetChild(0).GetComponent<Animator>();
	}

	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;//lock mouse
		hotbarCanvasClone = Instantiate(hotbarCanvas, Vector2.zero, Quaternion.identity);

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
			commandInput();
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	/*↓ maybe change in the future*/
	List<string> blockList = new List<string>()
	{
		"Grass Block(Clone)",
		"Dirt Block(Clone)",
		"Stone Block(Clone)",
		"Oak Leaf Block(Clone)",
		"Oaak Log Block(Clone)",
	};


	//public Vector3 moving_vector;
	private MeshRenderer meshRenderer;

	[SerializeField] float movingSpeed = 10f;
	[SerializeField] float movingSpeedRate = 1.0f;
	[SerializeField] float movingSpeedRateInAir = 0.3f;
	public float jumpForce;

	private bool rightClick;//true when player rightclick a block

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
	public GameObject hotbarCanvas;
	public GameObject keyTCanvas;
	private GameObject inputField;
}

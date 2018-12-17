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

	private void OnTriggerEnter(Collider other)
	{
		animator.SetBool("Jump", false);
	}

	private void OnTriggerExit(Collider other)
	{
		onGround = false;
	}

	private bool land = true;
	private void prepareForFall()
	{
		if (land)
		{
			land = false;
			Ray ray = new Ray(transform.position, new Vector3(0, -1, 0) + transform.position);

			//ignore "ignore RayCast" layer
			int raylayerMask = 1 << 2;
			raylayerMask = ~raylayerMask;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.8f, raylayerMask))
			{
				land = true;
			}
		}
		else
		{
			Ray ray = new Ray(transform.position, new Vector3(0, -1, 0) + transform.position);
			Debug.DrawLine(transform.position, new Vector3(0, -1, 0) + transform.position, Color.red);

			//ignore "ignore RayCast" layer
			int raylayerMask = 1 << 2;
			raylayerMask = ~raylayerMask;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.8f, raylayerMask))
			{
				land = true;
				animator.SetBool("Jump", false);
			}

		}
	}//use for animator

	private void RightMouseClick()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Ray Looking = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit LookingDirection;
			int rayLayerMask = ~(1 << 2);
			if (Physics.Raycast(Looking, out LookingDirection,spectMode==0?8.0f: 5.0f, rayLayerMask))
			{
				int choosingItem = HotBarInventory.GetComponent<hotbar>().selectItemSlot;
				if (LookingDirection.point.x - LookingDirection.collider.transform.position.x
					== LookingDirection.collider.transform.localScale.x / 2)
				{
					ChooseBlockToPlace(choosingItem, new Vector3(LookingDirection.collider.transform.position.x + 1, LookingDirection.collider.transform.position.y, LookingDirection.collider.transform.position.z));
				}
				else if (LookingDirection.point.x - LookingDirection.collider.transform.position.x
					== -(LookingDirection.collider.transform.localScale.x / 2))
				{
					ChooseBlockToPlace(choosingItem, new Vector3(LookingDirection.collider.transform.position.x - 1, LookingDirection.collider.transform.position.y, LookingDirection.collider.transform.position.z));
				}
				else if (LookingDirection.point.y - LookingDirection.collider.transform.position.y
					== LookingDirection.collider.transform.localScale.y / 2)
				{
					ChooseBlockToPlace(choosingItem, new Vector3(LookingDirection.collider.transform.position.x, LookingDirection.collider.transform.position.y + 1, LookingDirection.collider.transform.position.z));
				}
				else if (LookingDirection.point.y - LookingDirection.collider.transform.position.y
					== -(LookingDirection.collider.transform.localScale.y / 2))
				{
					ChooseBlockToPlace(choosingItem, new Vector3(LookingDirection.collider.transform.position.x, LookingDirection.collider.transform.position.y - 1, LookingDirection.collider.transform.position.z));
				}
				else if (LookingDirection.point.z - LookingDirection.collider.transform.position.z
					== LookingDirection.collider.transform.localScale.z / 2)
				{
					ChooseBlockToPlace(choosingItem, new Vector3(LookingDirection.collider.transform.position.x, LookingDirection.collider.transform.position.y, LookingDirection.collider.transform.position.z + 1));
				}
				else if (LookingDirection.point.z - LookingDirection.collider.transform.position.z
					== -(LookingDirection.collider.transform.localScale.z / 2))
				{
					ChooseBlockToPlace(choosingItem, new Vector3(LookingDirection.collider.transform.position.x, LookingDirection.collider.transform.position.y, LookingDirection.collider.transform.position.z - 1));
				}
			}
		}
	}

	private void CheckEmptyBlank()
	{
		for (int index = 0; index <= InventoryBlockName.Count - 1; ++index)
		{
			if (InventoryBlockName[index] == null || InventoryBlockAmount[index] <= 0)
			{
				InventoryBlockName.RemoveAt(index);
				InventoryBlockAmount.RemoveAt(index);
				break;
			}
			++index;
		}
	}

	private void ChooseBlockToPlace(int choosingItem, Vector3 position)
	{
		if (InventoryBlockName.Count - 1 >= choosingItem)
		{
			GameObject BlockNewPlaced;
			switch (InventoryBlockName[choosingItem])
			{
				case "Grass Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().grass);
					break;
				case "Dirt Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().dirt);
					break;
				case "Stone Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().stone);
					break;
				case "Oak Leaf Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().oakLeaves);
					break;
				case "Oak Log Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().oakLog);
					break;
				case "Brich Leaf Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().brichLeaves);
					break;
				case "Brich Log Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().brichLog);
					break;
				case "Sand Block(Clone)":
					BlockNewPlaced = Instantiate(BlockList.GetComponent<GroundGeneration>().sand);
					break;
				default:
					BlockNewPlaced = null;
					break;
			}
			--InventoryBlockAmount[choosingItem];
			--HotBarInventory.GetComponent<hotbar>().InventoryBlockAmount[choosingItem];
			if (InventoryBlockAmount[choosingItem] <= 0)
			{
				InventoryBlockAmount.RemoveAt(choosingItem);
				InventoryBlockName.RemoveAt(choosingItem);
				HotBarInventory.GetComponent<hotbar>().InventoryBlockAmount.RemoveAt(choosingItem);
				HotBarInventory.GetComponent<hotbar>().InventoryBlockName.RemoveAt(choosingItem);
			}
			if(BlockNewPlaced != null)
			{
				BlockNewPlaced.transform.position = position;
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
			int raylayerMask = ~(1 << 2);

			if (Physics.Raycast(ray, out rh, spectMode == 0 ? 8.0f : (spectMode == 1 ? 5.0f :-3.0f), raylayerMask))//2:ignore RayCast
			{
				foreach (string blockname in blockList)
				{
					if (rh.collider.name == blockname)//will be rh.collider.tag
					{
						NewBlockName = rh.collider.name;
						NewBlockPicked = false;
						if (InventoryBlockName.Count == 0)
						{
							InventoryBlockName.Add(rh.collider.name);
							InventoryBlockAmount.Add(1);
						}
						else
						{
							int ItemIndex = 0;
							bool picked = false;
							foreach (string BlockName in InventoryBlockName)
							{
								if (rh.collider.name == BlockName)
								{
									if (InventoryBlockAmount[ItemIndex] >= 64)
									{
										++ItemIndex;
										continue;
									}
									else
									{
										++InventoryBlockAmount[ItemIndex];
										picked = !picked;
									}
									break;
								}
								++ItemIndex;
							}
							if (!picked && InventoryBlockName.Count < 9)
							{
								InventoryBlockName.Add(rh.collider.name);
								InventoryBlockAmount.Add(1);
							}
						}
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
			pitch = pitch > 90 ? 90 : pitch;
			pitch = pitch < -90 ? -90 : pitch;

			transform.GetChild(0).eulerAngles = new Vector3(0, yaw, 0.0f);
			transform.GetChild(0).GetChild(0).eulerAngles = new Vector3(pitch, yaw, 0.0f);

			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).eulerAngles = new Vector3(pitch, yaw, 0.0f);//head
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
			GetComponent<Rigidbody>().velocity = (playerFront(playerGoFront) + playerLeft(playerGoLeft)).normalized * movingSpeed * movingSpeedRate;
		}
		else if (GetComponent<Rigidbody>().velocity.y < 0.5f && GetComponent<Rigidbody>().velocity.magnitude < 0.8f)
		{
			GetComponent<Rigidbody>().velocity += (playerFront(playerGoFront) + playerLeft(playerGoLeft)).normalized * movingSpeed * movingSpeedRate * movingSpeedRateInAir;
		}
	}

	private void moveControl()
	{

		animator.SetFloat("Speed", 0);
		playerGoFront = playerGoLeft = 0;
		if (Input.GetKey(KeyCode.W))
		{
			playerGoFront = 1;
			animator.SetFloat("Speed", 1);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			playerGoFront = 2;
			animator.SetFloat("Speed", -1);
		}

		if (Input.GetKey(KeyCode.A))
		{
			playerGoLeft = 1;
			animator.SetFloat("Speed", 1);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			playerGoLeft = 2;
			animator.SetFloat("Speed", 1);
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
			//GetComponent<Rigidbody>().AddForce(0, jumpForce * GetComponent<Rigidbody>().mass, 0);
			GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpForce, GetComponent<Rigidbody>().velocity.z);
			onGround = false;
			animator.SetBool("Jump", true);
		}
	}

	private void OnCollisionEnter(Collision obj)
	{
		if(obj.collider.name == "Slime(Clone)")
		{
			Debug.Log("Fuck");
			Vector3 bounceDirection = transform.position - obj.collider.transform.position;
			GetComponent<Rigidbody>().AddForce(bounceDirection.x * 550, 250, bounceDirection.z * 550, ForceMode.Impulse);
			GetComponent<AudioSource>().Play();

			Cursor.lockState = CursorLockMode.None;
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}

	GameObject pauseCanvasClone;
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
					//hotbarCanvasClone = Instantiate(hotbarCanvas,new Vector (146, 44, 0), Quaternion.identity);
					//hotbarCanvas = Instantiate(hotbarCanvas);
				}
			}
			else
			{
				Time.timeScale = 0f;
				pauseCanvasClone = Instantiate(pauseCanvas, Vector2.zero, Quaternion.identity);
				//Destroy(hotbarCanvas);
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

	private void spectChange()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			switch (++spectMode)
			{
				case 0:
					transform.GetChild(0).GetChild(0).GetChild(0).transform.localPosition = new Vector3(0, 0, -2);
					transform.GetChild(0).GetChild(0).GetChild(0).localRotation = new Quaternion(0, 0, 0, 0);
					break;
				case 1:
					transform.GetChild(0).GetChild(0).GetChild(0).transform.localPosition = new Vector3(0, 0, 0);

					int tar = 1 << 2;
					transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>().cullingMask = ~tar;
					break;
				case 2:
					transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>().cullingMask += 1 << 2;
					transform.GetChild(0).GetChild(0).GetChild(0).transform.localPosition = new Vector3(0, 0, 2f);
					transform.GetChild(0).GetChild(0).GetChild(0).localRotation = new Quaternion(0, 180, 0, 0);
					spectMode = -1;
					break;
			}
		}
	}

	private void timeChange()
	{
		time += Time.deltaTime * 30;
		time %= 1200;
		Light.transform.rotation = Quaternion.Euler(new Vector3(((int)time / 5 * 1.5f), 0, 0));
		//Debug.Log(time);
	}

	//GameObject body;
	private void Awake()
	{
		/*related to GetChild*/
		hotbarCanvas = Instantiate(hotbarCanvas);
		inputField = keyTCanvas.gameObject.transform.GetChild(0).GetChild(1).gameObject;
		animator = transform.GetChild(0).GetComponent<Animator>();
		//body = transform.GetChild(0).gameObject;
		HotBarInventory = GameObject.Find("HotBar");
		BlockList = GameObject.FindGameObjectWithTag("map");
	}

	// Use this for initialization
	void Start()
	{
		//lock mouse
		Cursor.lockState = CursorLockMode.Locked;
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
			prepareForFall();
			spectChange();
			timeChange();
			CheckEmptyBlank();
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}

		if (transform.position.y < 0)
			transform.position = new Vector3(10, 10, 10);
	}

	/*↓ maybe change in the future*/
	List<string> blockList = new List<string>()
	{
		"Grass Block(Clone)",
		"Dirt Block(Clone)",
		"Stone Block(Clone)",
		"Oak Leaf Block(Clone)",
		"Oak Log Block(Clone)",
		"Brich Leaf Block(Clone)",
		"Brich Log Block(Clone)",
		"Sand Block(Clone)"
	};

	//public Vector3 moving_vector;
	private MeshRenderer meshRenderer;

	[SerializeField]
	float movingSpeed = 10f;
	[SerializeField]
	float movingSpeedRate = 1.0f;
	[SerializeField]
	float movingSpeedRateInAir = 0.3f;
	public float jumpForce;

	//true when player rightclick a block
	private bool rightClick;
	//Ineventory
	public List<String> InventoryBlockName;
	public List<int> InventoryBlockAmount;
	//For Picking New Block
	public string NewBlockName;
	public bool NewBlockPicked = true;
	//For Placing block
	GameObject HotBarInventory;
	GameObject BlockList;

	public bool onGround = false; //if player standing on ground
	[SerializeField]
	bool canDestory = true; //if player can destroy blocks

	Animator animator;

	///
	public float speedH = 2.0f;
	public float speedV = 2.0f;

	[SerializeField]
	float yaw = 0.0f;
	[SerializeField]
	float pitch = 0.0f;
	///

	[SerializeField]
	bool lockMouse = true;
	public GameObject pauseCanvas;//Pause Canvas
	public Canvas hotbarCanvas;
	public GameObject keyTCanvas;
	private GameObject inputField;

	private int spectMode = 0;

	public float time = 0;
	[SerializeField]
	GameObject Light;
}

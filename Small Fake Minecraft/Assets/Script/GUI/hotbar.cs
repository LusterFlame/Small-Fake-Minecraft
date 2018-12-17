using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hotbar : MonoBehaviour {

	private void select()
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			mouseWheelChange(1);
		}else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			mouseWheelChange(-1);
		}
	}

	private void mouseWheelChange(int value)
	{
		selectItemSlot += value;
		selectItemSlot %= 9;
		if (selectItemSlot < 0)
		{
			selectItemSlot += 9;
		}
		Debug.Log(selectItemSlot);
	}

	private void GetNewBlock()
	{
		string NewBlock = PlayerInventory.GetComponent<playerCtrl>().NewBlockName;
		if (PlayerInventory.GetComponent<playerCtrl>().NewBlockPicked == false)
		{
			if (InventoryBlockName.Count == 0)
			{
				InventoryBlockName.Add(NewBlock);
				InventoryBlockAmount.Add(1);
			}
			else
			{
				int ItemIndex = 0;
				bool picked = false;
				foreach (string BlockName in InventoryBlockName)
				{
					if (NewBlock == BlockName)
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
					InventoryBlockName.Add(NewBlock);
					InventoryBlockAmount.Add(1);
				}
			}
			PlayerInventory.GetComponent<playerCtrl>().NewBlockPicked = true;
		}
	}

	private void CheckEmptyBlank()
	{
		for(int index = 0;index <= InventoryBlockName.Count - 1;++index)
		{
			if(InventoryBlockName[index] == null || InventoryBlockAmount[index] <= 0)
			{
				InventoryBlockName.RemoveAt(index);
				InventoryBlockAmount.RemoveAt(index);
				break;
			}
			++index;
		}
	}


	void Awake()
	{
		PlayerInventory = GameObject.FindGameObjectWithTag("Player");
	}

	// Use this for initialization
	void Start () {
		for(int temp = 0;temp <= 8;++temp)
		{
			Item[temp] = Instantiate(Item[temp]);
			Item[temp].transform.SetParent(transform);
			Item[temp].transform.position = new Vector3(temp * 80 + 19, Item[temp].transform.parent.position.y - 19, 100);
			ItemCount[temp] = Instantiate(ItemCount[temp]);
			ItemCount[temp].transform.SetParent(transform);
			ItemCount[temp].transform.position = new Vector3(temp * 80 + 36, ItemCount[temp].transform.parent.position.y - 40, 100);
		}
		HotBarSelected = Instantiate(HotBarSelected);
		HotBarSelected.transform.SetParent(transform);
		transform.position = new Vector3((Screen.width - 728) / 2, 88, 0);
	}

	// Update is called once per frame
	void Update() {
		select();
		GetNewBlock();
		CheckEmptyBlank();
		RefreshHotBarImage();
		RefreshHotBarItemCount();
		HotBarSelected.transform.position = new Vector3((Screen.width - 728) / 2 + selectItemSlot * 80, 0, 0);
	}

	private void RefreshHotBarImage()
	{
		for(int temp = 0;temp < 9;++temp)
		{
			Item[temp].texture = Base.texture;
		}

		for(int temp = 0;temp <= InventoryBlockName.Count - 1; ++temp)
		{
			switch(InventoryBlockName[temp])
			{
				case "Grass Block(Clone)":
					Item[temp].texture = GrassBlock.texture;
					break;
				case "Dirt Block(Clone)":
					Item[temp].texture = DirtBlock.texture;
					break;
				case "Stone Block(Clone)":
					Item[temp].texture = StoneBlock.texture;
					break;
				case "Oak Leaf Block(Clone)":
					Item[temp].texture = OakLeafBlock.texture;
					break;
				case "Oak Log Block(Clone)":
					Item[temp].texture = OakLogBlock.texture;
					break;
				case "Brich Leaf Block(Clone)":
					Item[temp].texture = BrichLeafBlock.texture;
					break;
				case "Brich Log Block(Clone)":
					Item[temp].texture = BrichLogBlock.texture;
					break;
				case "Sand Block(Clone)":
					Item[temp].texture = SandBlock.texture;
					break;
				default:
					Item[temp].texture = Base.texture;
					break;
			}
		}
	}
	private void RefreshHotBarItemCount()
	{
		for (int temp = 0; temp < 9; ++temp)
		{
			ItemCount[temp].text = "";
		}
		for (int temp = 0;temp < InventoryBlockAmount.Count;++temp)
		{
			if(InventoryBlockAmount[temp] != 0)
			ItemCount[temp].text = InventoryBlockAmount[temp].ToString();
		}
	}

	public int selectItemSlot;
	public RawImage HotBarSelected;
	private RectTransform HotBarRec;
	public List<string> InventoryBlockName;
	public List<int> InventoryBlockAmount;
	GameObject PlayerInventory;

	public RawImage Base;

	public RawImage GrassBlock;
	public RawImage StoneBlock;
	public RawImage DirtBlock;
	public RawImage OakLogBlock;
	public RawImage OakLeafBlock;
	public RawImage BrichLogBlock;
	public RawImage BrichLeafBlock;
	public RawImage SandBlock;

	[SerializeField] private RawImage[] Item = new RawImage[9];
	[SerializeField] private Text[] ItemCount = new Text[9];
}

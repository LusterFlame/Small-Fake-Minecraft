﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundGeneration : MonoBehaviour {

	public GameObject bedrock;
	public GameObject grass;
	public GameObject dirt;
	public GameObject stone;
	public GameObject oakLeaves;
	public GameObject oakLog;
	public GameObject brichLeaves;
	public GameObject brichLog;
	public GameObject sand;
	public GameObject andesite;
	public GameObject diorite;
	public GameObject granite;
	private GameObject bedrockCollider;
	[SerializeField] private int[,] height;

	void Awake()
	{
		height = new int[64, 64];
	}
	// Use this for initialization
	void Start() {

		//Create Bedrock plane for bottom
		bedrockCollider = new GameObject();
		bedrockCollider.AddComponent<BoxCollider>();
		bedrockCollider.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
		bedrockCollider.GetComponent<BoxCollider>().size = new Vector3(64, 1, 64);
		placeBedrock();

		//Decide Ground height for every block
		setHeight(height);
		// Original Height Decided

		//place dirt blocks
		placeGround(height);

		//Random sands
		for (int temp = 0; temp <= 19; ++temp)
		{
			PlaceSand(height, Random.Range(-25, 25), Random.Range(-25, 25));
		}
		//Place trees
		placeTree(height, oakLog, oakLeaves);
		placeTree(height, brichLog, brichLeaves);

	}

	private void setHeight(int[,] height)
	{
		height[0, 0] = Random.Range(4, 12);
		for (int temp = 0; temp < 64; temp++)
			for (int temp2 = 0; temp2 < 64; temp2++)
			{
				if (height[temp, temp2] == 0)
				{
					if (temp == 0)
					{
						switch (Random.Range(0, 15))
						{
							case 0:
								if (!(height[temp, temp2 - 1] == 1))
									height[temp, temp2] = height[temp, temp2 - 1] - 1;
								else
									height[temp, temp2] = height[temp, temp2 - 1];
								break;
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12:
							case 13:
								height[temp, temp2] = height[temp, temp2 - 1];
								break;
							case 14:
								height[temp, temp2] = height[temp, temp2 - 1] + 1;
								break;
						}
					}
					else if (temp2 == 0)
					{
						switch (Random.Range(0, 15))
						{
							case 0:
								if (!(height[temp - 1, temp2] == 1))
									height[temp, temp2] = height[temp - 1, temp2] - 1;
								else
									height[temp, temp2] = height[temp - 1, temp2];
								break;
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12:
							case 13:
								height[temp, temp2] = height[temp - 1, temp2];
								break;
							case 14:
								height[temp, temp2] = height[temp - 1, temp2] + 1;
								break;
						}
					}
					else
					{
						int Range = height[temp - 1, temp2] - height[temp, temp2 - 1];
						switch (Range)
						{
							case 2:
							case -2:
								height[temp, temp2] = (height[temp - 1, temp2] + height[temp, temp2 - 1]) / 2;
								break;
							case 1:
							case -1:
								switch (Random.Range(0, 2))
								{
									case 0:
										height[temp, temp2] = height[temp - 1, temp2];
										break;
									case 1:
										height[temp, temp2] = height[temp, temp2 - 1];
										break;
								}
								break;
							case 0:
								switch (Random.Range(0, 15))
								{
									case 0:
										if (!(height[temp - 1, temp2] == 1))
											height[temp, temp2] = height[temp - 1, temp2] - 1;
										else
											height[temp, temp2] = height[temp - 1, temp2];
										break;
									case 1:
									case 2:
									case 3:
									case 4:
									case 5:
									case 6:
									case 7:
									case 8:
									case 9:
									case 10:
									case 11:
									case 12:
									case 13:
										height[temp, temp2] = height[temp - 1, temp2];
										break;
									case 14:
										height[temp, temp2] = height[temp - 1, temp2] + 1;
										break;
								}
								break;
						}
					}
				}
			}
		//Make it smoother
		for (int temp = 0; temp < 10; ++temp)
			flattening(height);
	}
	private void flattening(int[,] height)
	{
		for (int temp = 1; temp < 63; ++temp)
			for (int temp2 = 1; temp2 < 63; ++temp2)
			{
				List<int> aroundHeight = new List<int>()
				{
					height[temp - 1, temp2 - 1],
					height[temp - 1, temp2],
					height[temp - 1, temp2 + 1],
					height[temp, temp2 - 1],
					height[temp, temp2 + 1],
					height[temp + 1, temp2 - 1],
					height[temp + 1, temp2 - 1],
					height[temp + 1, temp2 - 1],
				};
				if (aroundHeight.Count(x => x == height[temp, temp2]) <= 1)
					height[temp, temp2] = aroundHeight[Random.Range(0, 8)];
			}
	}
	private void placeBedrock()
	{
		for (int temp = -31; temp <= 32; ++temp)
			for (int temp2 = -31; temp2 <= 32; ++temp2)
			{
				GameObject block = Instantiate(bedrock);
				Destroy(block.GetComponent<BoxCollider>());
				block.GetComponent<Transform>().position = new Vector3(temp, 0, temp2);
			}
	}
	private void placeGround(int[,] height)
	{
		for (int temp = -31; temp <= 32; ++temp)
			for (int temp2 = -31; temp2 <= 32; ++temp2)
			{
				Debug.Log(temp.ToString() + "is temp and" + temp2.ToString() + "is temp2");
				if (height[temp + 31, temp2 + 31] > 1)
				{
					if(height[temp + 31, temp2 + 31] > 2)
					{
						for (int temp3 = 1; temp3 < height[temp + 31, temp2 + 31] - 1; ++temp3)
						{
							GameObject block2;
							switch(Random.Range(0, 7))
							{
								case 0:
								case 4:
								case 5:
								case 6:
									block2 = Instantiate(stone);
									break;
								case 1:
									block2 = Instantiate(diorite);
									break;
								case 2:
									block2 = Instantiate(andesite);
									break;
								case 3:
									block2 = Instantiate(granite);
									break;
								default:
									block2 = Instantiate(stone);
									break;
							}
							block2.transform.position = new Vector3(temp, temp3, temp2);
						}
					}
					GameObject block4 = Instantiate(dirt);
					block4.transform.position = new Vector3(temp, height[temp + 31, temp2 + 31] - 1, temp2);
				}
				GameObject block3 = Instantiate(grass);
				block3.transform.position = new Vector3(temp, height[temp + 31, temp2 + 31], temp2);
				Debug.Log("is it before the error mesg?");
			}
	}
	private void placeTree(int[,] height, GameObject WoodType, GameObject LeafType)
	{
		int TotalTree = Random.Range(8, 20);
		for (int temp = 0; temp < TotalTree; ++temp)
		{
			Debug.Log("This is the " + (temp + 1).ToString() + "tree");
			int TreeX = Random.Range(-29, 30) + 31;
			int TreeZ = Random.Range(-29, 30) + 31;
			Debug.Log("TreeX is " + TreeX.ToString() + "and TreeZ is " + TreeZ.ToString());
			int Baseheight = height[TreeX, TreeZ] + 1;
			int TreeHeight = Random.Range(4, 6);
			for (int temp2 = Baseheight; temp2 <= TreeHeight + Baseheight; ++temp2)
			{
				GameObject Block5 = Instantiate(WoodType);
				Block5.transform.position = new Vector3(TreeX - 31, temp2, TreeZ - 31);
				switch (TreeHeight - (temp2 - Baseheight))
				{
					case 2:
						PlaceLeaves(TreeX - 31, TreeZ - 31, temp2, 2, LeafType);
						break;
					case 1:
						PlaceLeaves(TreeX - 31, TreeZ - 31, temp2, 1, LeafType);
						break;
					case 0:
						PlaceLeaves(TreeX - 31, TreeZ - 31, temp2, 0, LeafType);
						break;
					default:
						break;
				}
			}
		}
	}

	private void PlaceLeaves(int TreeX, int TreeZ, int TreeY, int level, GameObject LeafType)
	{
		if (level <= 0)
		{
					for (int temp = 0; temp <= 1; ++temp)
					{
						GameObject Block10 = Instantiate(LeafType);
						GameObject Block11 = Instantiate(LeafType);
						GameObject Block12 = Instantiate(LeafType);
						GameObject Block13 = Instantiate(LeafType);
						Block10.transform.position = new Vector3(TreeX + 1, TreeY + temp, TreeZ);
						Block11.transform.position = new Vector3(TreeX - 1, TreeY + temp, TreeZ);
						Block12.transform.position = new Vector3(TreeX, TreeY + temp, TreeZ + 1);
						Block13.transform.position = new Vector3(TreeX, TreeY + temp, TreeZ - 1);
					}
					GameObject Block14 = Instantiate(LeafType);
					Block14.transform.position = new Vector3(TreeX, TreeY + 1, TreeZ);
		}
		else
		{
			for (int temp = 1; temp <= level; ++temp)
				for (int temp2 = level * -1; temp2 <= level; ++temp2)
				{
					GameObject Block6 = Instantiate(LeafType);
					Block6.transform.position = new Vector3(TreeX + temp, TreeY, TreeZ + temp2);
				}
			for (int temp = -1; temp * -1 <= level; --temp)
				for (int temp2 = level * -1; temp2 <= level; ++temp2)
				{
					GameObject Block7 = Instantiate(LeafType);
					Block7.transform.position = new Vector3(TreeX + temp, TreeY, TreeZ + temp2);
				}
			for (int temp = 1; temp <= level; ++temp)
			{
				GameObject Block8 = Instantiate(LeafType);
				Block8.transform.position = new Vector3(TreeX, TreeY, TreeZ + temp);
				GameObject Block9 = Instantiate(LeafType);
				Block9.transform.position = new Vector3(TreeX, TreeY, TreeZ + temp * -1);
			}
		}
	}

	private void PlaceSand(int[,] height, int SandX, int SandZ)
	{
		for(int temp = -1;temp < 2;++temp)
			for(int temp2 = -1;temp2 < 2;++temp2)
			{
				++height[SandX + 31 + temp, SandZ + 31 + temp2];
				GameObject Block16 = Instantiate(sand);
				Block16.transform.position = new Vector3(SandX + temp, height[SandX + 31 + temp, SandZ + 31 + temp2] + 1, SandZ + temp2);
			}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

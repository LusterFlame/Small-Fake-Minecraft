using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundGeneration : MonoBehaviour {

	public GameObject bedrock;
	public GameObject grass;
	public GameObject dirt;
	public GameObject stone;
	private GameObject bedrockCollider;

	// Use this for initialization
	void Start () {

		//Create Bedrock plane for bottom
		bedrockCollider = new GameObject();
		bedrockCollider.AddComponent<BoxCollider>();
		bedrockCollider.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
		bedrockCollider.GetComponent<BoxCollider>().size = new Vector3(64, 1, 64);
		placeBedrock();

		//Decide Ground height for every block
		int[,] height = new int[64, 64];
		setHeight(height);
		// Original Height Decided

		//place dirt blocks
		placeGround(height);


	}

	private void setHeight(int[,] height)
	{
		height[0, 0] = Random.Range(1, 4);
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
		for(int temp = 0;temp < 3;++temp)
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
				for (int temp3 = 1; temp3 < height[temp + 31, temp2 + 31] - 1; ++temp3)
				{
					GameObject block2 = Instantiate(stone);
					block2.transform.position = new Vector3(temp, temp3, temp2);
				}
				for (int temp3 = height[temp + 31, temp2 + 31] - 1; temp3 < height[temp + 31, temp2 + 31]; ++temp3)
				{
					GameObject block4 = Instantiate(dirt);
					block4.transform.position = new Vector3(temp, temp3, temp2);
				}
				GameObject block3 = Instantiate(grass);
				block3.transform.position = new Vector3(temp, height[temp + 31, temp2 + 31], temp2);
			}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

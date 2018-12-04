using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGeneration : MonoBehaviour {

	public GameObject bedrock;
	public GameObject grass;
	public GameObject dirt;
	private GameObject bedrockCollider;

	// Use this for initialization
	void Start () {

		//Create Bedrock plane for bottom
		bedrockCollider = new GameObject();
		bedrockCollider.AddComponent<BoxCollider>();
		bedrockCollider.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
		bedrockCollider.GetComponent<BoxCollider>().size = new Vector3(64, 1, 64);
		//Debug.Log(Bedrock_collider.GetComponent<BoxCollider>().center);
		for (int temp = -31; temp <= 32; ++temp)
			for (int temp2 = -31; temp2 <= 32; ++temp2)
			{
				GameObject block = Instantiate(bedrock);
				Destroy(block.GetComponent<BoxCollider>());
				block.GetComponent<Transform>().position = new Vector3(temp, 0, temp2);
			}

		//Decide Ground height for every block
		int[,] height = new int[64, 64];
		setHeight(height);
		// Original Height Decided
		// But not flat enough

		//place dirt blocks
		for(int temp = -31;temp <= 32;++temp)
			for(int temp2 = -31;temp2 <= 32;++temp2)
			{
				for (int temp3 = 1; temp3 < height[temp + 31, temp2 + 31];++temp3)
				{
					GameObject block2 = Instantiate(dirt);
					block2.transform.position = new Vector3(temp, temp3, temp2);
				}
				GameObject block3 = Instantiate(grass);
				block3.transform.position = new Vector3(temp, height[temp + 31, temp2 + 31], temp2);
			}


	}

	private void setHeight(int[,] Height)
	{
		Height[0, 0] = Random.Range(1, 4);
		for (int temp = 0; temp < 64; temp++)
			for (int temp2 = 0; temp2 < 64; temp2++)
			{
				if (Height[temp, temp2] == 0)
				{
					if (temp == 0)
					{
						switch (Random.Range(0, 10))
						{
							case 0:
								if (!(Height[temp, temp2 - 1] == 1))
									Height[temp, temp2] = Height[temp, temp2 - 1] - 1;
								else
									Height[temp, temp2] = Height[temp, temp2 - 1];
								break;
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
								Height[temp, temp2] = Height[temp, temp2 - 1];
								break;
							case 9:
								Height[temp, temp2] = Height[temp, temp2 - 1] + 1;
								break;
						}
					}
					else if (temp2 == 0)
					{
						switch (Random.Range(0, 10))
						{
							case 0:
								if (!(Height[temp - 1, temp2] == 1))
									Height[temp, temp2] = Height[temp - 1, temp2] - 1;
								else
									Height[temp, temp2] = Height[temp - 1, temp2];
								break;
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
								Height[temp, temp2] = Height[temp - 1, temp2];
								break;
							case 9:
								Height[temp, temp2] = Height[temp - 1, temp2] + 1;
								break;
						}
					}
					else
					{
						int Range = Height[temp - 1, temp2] - Height[temp, temp2 - 1];
						switch (Range)
						{
							case 2:
							case -2:
								Height[temp, temp2] = (Height[temp - 1, temp2] + Height[temp, temp2 - 1]) / 2;
								break;
							case 1:
							case -1:
								switch (Random.Range(0, 2))
								{
									case 0:
										Height[temp, temp2] = Height[temp - 1, temp2];
										break;
									case 1:
										Height[temp, temp2] = Height[temp, temp2 - 1];
										break;
								}
								break;
							case 0:
								switch (Random.Range(0, 10))
								{
									case 0:
										if (!(Height[temp - 1, temp2] == 1))
											Height[temp, temp2] = Height[temp - 1, temp2] - 1;
										else
											Height[temp, temp2] = Height[temp - 1, temp2];
										break;
									case 1:
									case 2:
									case 3:
									case 4:
									case 5:
									case 6:
									case 7:
									case 8:
										Height[temp, temp2] = Height[temp - 1, temp2];
										break;
									case 9:
										Height[temp, temp2] = Height[temp - 1, temp2] + 1;
										break;
								}
								break;
						}
					}
				}
			}
	}
	private void setBedrock()
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGeneration : MonoBehaviour {

	public GameObject Bedrock;
	public GameObject Grass;
	public GameObject dirt;

	// Use this for initialization
	void Start () {
		for (int temp = -64; temp <= 64; ++temp)
			for (int temp2 = -64; temp2 <= 64; ++temp2)
			{
				GameObject Block = Instantiate(Bedrock);
				//Destroy(Block.GetComponent<BoxCollider>());
				Block.GetComponent<Transform>().position = new Vector3(temp, 0, temp2);
			}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

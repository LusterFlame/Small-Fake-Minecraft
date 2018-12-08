using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		GameObject selectSquare = transform.GetChild(0).gameObject;	
		selectSquare.GetComponent<RectTransform>().transform.localPosition = new Vector3(-530.99f + selectItemSlot * 133, 0, 0);
	}

	private void mouseWheelChange(int value)
	{
		selectItemSlot += value;
		selectItemSlot %= 9;
		if (selectItemSlot < 0)
		{
			selectItemSlot += 9;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		select();
	}

	[SerializeField] int selectItemSlot = 0;
}

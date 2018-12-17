using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour {

	void Awake()
	{
		Playerinfo = GameObject.Find("charCenter");
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update() {
		toward = Playerinfo.transform.position - transform.position;
		//Debug.Log(toward);
		++count;
		if(count == 120)
		{
			GetComponent<Rigidbody>().AddForce(toward.x, 15, toward.z);
			GetComponent<AudioSource>().Play();
			count = 0;
		}

		if (Playerinfo.GetComponent<playerCtrl>().time > 200 && Playerinfo.GetComponent<playerCtrl>().time < 750 || transform.position.y >= 20)
			Destroy(this.gameObject);
	}

	int count;
	private GameObject Playerinfo;
	[SerializeField]
	private Vector3 toward;
}

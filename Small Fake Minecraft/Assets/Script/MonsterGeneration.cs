using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneration : MonoBehaviour {
	void Awake (){
		Playerinfo = GameObject.Find("charCenter");
	}

	// Use this for initialization
	void Start () {
		SlimeCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time = Playerinfo.GetComponent<playerCtrl>().time;
		SpawnSlime();
	}

	void SpawnSlime()
	{
		if (time >= 750 && Mathf.Abs(time % 25 - 25) < 1)
		{
			if (Random.Range(0, 3) == 0 && SlimeCount < 15)
			{
				GameObject NewSlime = Instantiate(Slime);
				NewSlime.transform.position = new Vector3(Random.Range(-30, 30), 10, Random.Range(-30, 30));
				++SlimeCount;
				SlimeList.Add(Slime);
			}
		}
		else if (time >= 200 && time <= 750)
		{
			for (int temp = SlimeList.Count - 1; temp >= 0; --temp)
			{
				SlimeList.RemoveAt(temp);
			}
			SlimeCount = 0;
		}
	}

	[SerializeField] private List<GameObject> SlimeList;
	public GameObject Slime;
	private GameObject Playerinfo;
	private float time;
	public int SlimeCount;

}

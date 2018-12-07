using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour, IPointerClickHandler {
	public void OnPointerClick(PointerEventData e)
	{
		SceneManager.LoadScene(1);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

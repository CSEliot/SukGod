using UnityEngine;
using System.Collections;

public class chanting : MonoBehaviour {

	public GameObject characterRB; 
	public string chantKey;
	public AudioSource chant;
	public float chantDuration = 10f;
	private bool isChanting;



	// Use this for initialization
	void Start () {
		isChanting = false;
	}

	// Update is called once per frame
	void Update () {

	}


	public void startChanting(){
		if (Input.GetButton (chantKey))
			isChanting = true;
	}

	public void startTimer(){
		
		chant.Play ();  //Start Chanting

		for (float time = chantDuration; time > 0; time--);  //loop for chant duration

		chant.Stop ();  //Start Chanting
	
	}
}
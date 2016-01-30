using UnityEngine;
using System.Collections;

public class ChantBehavior : MonoBehaviour {

	private GameObject characterRB; 
	public string chantKey;
	public AudioSource chant;
	public float chantDuration = 10f;
	private bool isChanting;



	// Use this for initialization
	void Start () {
		isChanting = false;
		characterRB = this.gameObject;
	}

	// Update is called once per frame
	void Update () {
		startChanting ();
	}


	public void startChanting(){
		if (Input.GetButton (chantKey)) {
			isChanting = true;
			startTimer ();
			isChanting = false;
		
		}
	}

	public void startTimer(){
		
		chant.Play ();  //Start Chanting

		for (float time = chantDuration; time > 0; time--);  //loop for chant duration

		chant.Stop ();  //Start Chanting
	}

	public bool chantStatus(){
		return isChanting;
	}
}
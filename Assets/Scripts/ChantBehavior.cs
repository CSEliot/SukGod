/**
 * @author Chris Ridgely
 * @updated 12:09 Jan 30
 */

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
		if (Input.GetButton (chantKey) && isChanting==false)
			StartCoroutine(startChanting ());
	}


	public IEnumerator startChanting(){
		isChanting = true;
		gameObject.GetComponent<CloneBehavior> ().recordLocation();
		chant.Play (); //Start Chanting
		yield return new WaitForSeconds (chantDuration);
		chant.Stop (); //Stops Chanting

		isChanting = false;

	}

	public bool chantStatus(){
		return isChanting;
	}
}
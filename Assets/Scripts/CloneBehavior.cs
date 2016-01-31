using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class CloneBehavior : MonoBehaviour {

	public Vector3[] locationArray;
	public Quaternion[] rotationArray;
	private GameObject player;
	private float chantDuration;
	public GameObject clonePlayer;
	public float speed = .1f;
	public float recordDelay = 1f;
	private int dynamicArraySize;

	// Use this for initialization
	void Start () {
		chantDuration = gameObject.GetComponent<ChantBehavior> ().chantDuration;
		dynamicArraySize = (int)(chantDuration / recordDelay);
		player = gameObject;
		//locationArray = new Vector3[dynamicArraySize];
		//rotationArray = new Quaternion[dynamicArraySize];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void recordLocation(){
		// Dynamic Array size determines maximum size depending on 
		// chant length per waypoint delay
		locationArray = new Vector3[dynamicArraySize];
		rotationArray = new Quaternion[dynamicArraySize];

		StartCoroutine(recordLocationHelper ());
	}

	public void executeDeathCycle(Vector3[] locs, Quaternion[] rots, int pointsRecorded){
		Debug.Log ("Death Cycle:" + locs[0]);
		gameObject.transform.position = locs[0];
		gameObject.transform.rotation = rots [0];

		StartCoroutine(moveFunction(locs, rots, pointsRecorded));
		//StartCoroutine(rotateFunction(rots));
	}

	// This function is only to be called in the clone
	public IEnumerator moveFunction(Vector3[] positions, Quaternion[] rotations, int pointsRecorded){
		int i = 0;

		//The clone will instantiate array equal to points recorded
		locationArray = new Vector3[pointsRecorded];
		rotationArray = new Quaternion[pointsRecorded];

		//Deep copy of array arguments
		for(int index = 0; index<pointsRecorded; index++){
			locationArray[index] = positions[index];
			rotationArray[index] = rotations[index];
		}


		Debug.Log ("Move Function" + gameObject.name);
		Vector3 a = gameObject.transform.position;
		Vector3 b = locationArray[i];
		Quaternion rotA = gameObject.transform.rotation;
		Quaternion rotB = rotationArray [i];

		float timeSinceStarted = 0f;
		while (i < pointsRecorded) {
			Debug.Log ("Lerping: " + i);
			timeSinceStarted += Time.deltaTime;
			gameObject.transform.position = Vector3.Lerp (a, b, timeSinceStarted);
			gameObject.transform.rotation = Quaternion.Lerp (rotA, rotB, timeSinceStarted);
			//If arrived, stop coroutine
			if (gameObject.transform.position == b && gameObject.transform.rotation == rotB) {
				a = gameObject.transform.position;
				b = locationArray[i];
				rotA = gameObject.transform.rotation;
				Debug.Log ("Rotation Array: " + i);
				rotB = rotationArray [i++];
				timeSinceStarted = 0f;
				//yield break;
			}
			yield return null;
			Debug.Log ("Return Null");
		}
	}

	public IEnumerator recordLocationHelper(){
		Debug.Log("Entered Record Location Helper");
		int i = 0;
		while (player.GetComponent<ChantBehavior> ().chantStatus ()) {
			
			Debug.Log ("Entered While Loop");
			Debug.Log (dynamicArraySize);
			locationArray[i] = (player.transform.position);
			rotationArray [i] = (player.transform.rotation);
			i++;
			yield return new WaitForSeconds (recordDelay);
		}

		Debug.Log ("Exited While Loop");
		GameObject clone = (GameObject)Instantiate (clonePlayer, transform.position, transform.rotation);
		clone.GetComponent<CloneBehavior> ().executeDeathCycle(locationArray, rotationArray, i);
	}

}

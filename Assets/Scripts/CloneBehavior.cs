using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class CloneBehavior : MonoBehaviour {

	public Vector3[] locationArray;
	private GameObject player;
	private float chantDuration;
	public GameObject clonePlayer;
	public float speed = 2f;
	public float recordDelay = 1f;
	private int dynamicArraySize;

	// Use this for initialization
	void Start () {
		chantDuration = gameObject.GetComponent<ChantBehavior> ().chantDuration;
		dynamicArraySize = (int)(chantDuration / recordDelay);
		player = gameObject;
		locationArray = new Vector3[dynamicArraySize+1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void recordLocation(){
		StartCoroutine(recordLocationHelper ());
	}

	public void executeDeathCycle(Vector3[] locs){
		Debug.Log ("Death Cycle:" + locs[0]);
		gameObject.transform.position = locs[0];

		StartCoroutine(moveFunction(locs));
	}

	public IEnumerator moveFunction(Vector3[] positions){
		int i = 0;

		Vector3 a = gameObject.transform.position;
		Vector3 b = positions[i];

		float timeSinceStarted = 0f;
		while (i < positions.Length) {
			Debug.Log ("Lerping: " + i);
			timeSinceStarted += Time.deltaTime;
			gameObject.transform.position = Vector3.Lerp (a, b, timeSinceStarted);
			//If arrived, stop coroutine
			if (gameObject.transform.position == b) {
				a = gameObject.transform.position;
				b = positions[i++];
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
			i++;
			yield return new WaitForSeconds (recordDelay);
		}

		Debug.Log ("Exited While Loop");
		GameObject clone = (GameObject)Instantiate (clonePlayer, transform.position, transform.rotation);
		clone.GetComponent<CloneBehavior> ().executeDeathCycle(locationArray);
	}

}

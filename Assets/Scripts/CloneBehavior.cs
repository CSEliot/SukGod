using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class CloneBehavior : MonoBehaviour {

	public Queue<Vector3> location;
	private GameObject player;
	public GameObject clonePlayer;

	// Use this for initialization
	void Start () {
		player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void recordLocation(){
		while (player.GetComponent<ChantBehavior> ().chantStatus)
			location.Enqueue (player.transform.position);
		
		GameObject clone = (GameObject)Instantiate (clonePlayer, transform.position, transform.rotation);
		clone.GetComponent<CloneBehavior> ().location = this.location;
	}

	public void executeDeathCycle(){
		player.transform.position = location.Dequeue ();

		StartCoroutine(moveFunction(location));
	}

	public IEnumerator moveFunction(Queue<Vector3> positions){
		Vector3 a = player.transform.position;
		Vector3 b = positions.Dequeue;

		float timeSinceStarted = 0f;
		while (true) {
			timeSinceStarted += Time.deltaTime;
			player.transform.position = Vector3.Lerp (a, b, timeSinceStarted * speed);

			//If arrived, stop coroutine
			if (player.transform.position == b && positions.Peek != null) {
				a = player.transform.position;
				b = positions.Dequeue;
				//yield break;
			} else
				yield break;

			yield return null;
		}
	}

}

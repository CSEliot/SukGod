using UnityEngine;
using System.Collections;

public class combatBehavior : MonoBehaviour {

	public int hitPoints = 3;
	public string attackKey = "Fire1";


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerStay(Collider other){
		if (Input.GetButton (attackKey) && this.gameObject.tag == "Red Player" && other.gameObject.tag == "Blue Player") {
			//play attack animation here

			other.SendMessageUpwards ("loseHealth", 1);
			Debug.Log("combatBehavior: Attack Key Received");
		}
	}

	public void loseHealth(){
		if (hitPoints > 0)
			hitPoints--;
		else
			Destroy (this.gameObject); //Changed in the future to incorporate 
	}
}

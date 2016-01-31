/**
 * @author Chris Ridgely
 * @updated 7:27 Jan 30
 */

using UnityEngine;
using System.Collections;

public class combatBehavior : MonoBehaviour {

	public int hitPoints = 3;
	public string attackKey = "Fire1";
	public float attackCooldown = 5f;
	public bool isAttacking;
	private bool canAttack;
	private BoxCollider hitbox;




	// Use this for initialization
	void Start () {
		isAttacking = false;
		canAttack = true;
		hitbox = gameObject.GetComponent<BoxCollider> ();
		hitbox.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown (attackKey) && canAttack) {
			hitbox.enabled = true;
			StartCoroutine(cooldown ());
		}
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("Trigger Entered");
		if(this.gameObject.tag == "Red Player" && other.gameObject.tag == "Blue Player")
			other.SendMessageUpwards ("loseHealth");
	}
		
	public void loseHealth(){
		if (hitPoints > 0) {
			hitPoints--;
			Debug.Log (hitPoints);
		} else {
			Destroy (this.gameObject); //Changed in the future to incorporate death animation
	
		}
	}

	public IEnumerator cooldown(){
		canAttack = false;
		yield return new WaitForSeconds (attackCooldown);
		canAttack = true;
		Debug.Log ("Cooldown Ended");

	}
}

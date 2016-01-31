using UnityEngine;
using System.Collections;

public class combatBehavior : MonoBehaviour {

	public int hitPoints = 3;
	public string attackKey = "Fire1";
	public float attackCooldown = 5f;
	public bool isAttacking;
	private bool canAttack;
	private BoxCollider hitbox;

    private PhotonView m_p;


	// Use this for initialization
	void Start () {
        m_p = GetComponent<PhotonView>();
		isAttacking = false;
		canAttack = true;
		hitbox = gameObject.GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		hitbox.enabled = false;
        if (Input.GetButtonDown(attackKey) && canAttack && gameObject.tag == "Red Player" && m_p.isMine)
        {
            GetComponent<PhotonView>().RPC("AttackRPC", PhotonTargets.All);
        }
        else
        {
            GetComponent<AnimationManager>().isAttacking(false);
        }
	}

    [PunRPC]
    void AttackRPC()
    {
        hitbox.enabled = true;
        GetComponent<AnimationManager>().isAttacking(true);
        StartCoroutine(cooldown());
    }

	void OnTriggerEnter(Collider other){
		Debug.Log ("Combat Trigger Entered: " + other.name);
		if(this.gameObject.tag == "Red Player" && other.gameObject.tag == "Blue Player")
			other.SendMessageUpwards ("loseHealth");
	}
		
	public void loseHealth(){
		if (hitPoints > 0) {
			hitPoints--;
			Debug.Log (hitPoints);
		} else {
            GetComponent<AnimationManager>().isDead(true);
            //Destroy (this.gameObject); //Changed in the future to incorporate death animation
		}
	}

	public IEnumerator cooldown(){
		canAttack = false;
		yield return new WaitForSeconds (attackCooldown);
		canAttack = true;
		Debug.Log ("Cooldown Ended");
	}
}

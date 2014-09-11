using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class MissileControl : MonoBehaviour {
	public bool inMotion = true; // Flag for activating the motion animationn
	
	float fireSpeed = 70f; // What speed should this missile move forward?
	float selfDestructTime = 1f; // Amount of time before self destructing
	bool hit = false; // Used to stop the selfDestruct and such
	
	Animator animator; // Stores the animator component

	// Use this for initialization
	void Start () {
		animator = this.gameObject.GetComponent<Animator>();
		if(animator == null)
		{
			Debug.LogError("Cant find the animator!");
		}
		
		if(inMotion)
		{
			// Activate the motion animation
			animator.SetTrigger("Shoot");
		}
		
		// Start self destruct timer
		StartCoroutine(SelfDestruct(selfDestructTime));
	}
	
	void FixedUpdate()
	{
		// If this is in motion, move it forward
		if(inMotion)
		{
			// Move the object forward
			rigidbody2D.velocity = transform.right * fireSpeed;
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
	//	Debug.Log("Missile Triggered! Object: " + collider.transform.name);
		
		// Do following things only if hit an enemy
		if(collider.transform.tag == "Enemy")
		{
	//		Debug.Log ("---> Contact with an enemy!");
			
			// Destroy the enemy
			collider.gameObject.GetComponent<EnemySpawn>().Attacked();
			
			// Activate missile on-hit stuff
			StartCoroutine(OnHit());
		}
	}
	
	// Do stuff when hit something
	IEnumerator OnHit()
	{
		// Flag that this object should stop moving
		inMotion = false;
		// Flag that this object has hit an enemy
		hit = true;
		
		// Activate the explosion animation
		animator.SetTrigger("Hit");
		
		// Make sure the missile can't destroy more than one enemy
		this.GetComponent<BoxCollider2D>().enabled = false;
		
		// Wait till the explosion ends
		yield return new WaitForSeconds(0.7f);
		
		// Destroy this missile for evah
		Destroy(this.gameObject);
	}
	
	// Destroy self in certain amount of time, just in case
	IEnumerator SelfDestruct(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		// Don't self destruct if already hit an enemy
		if(!hit)
		{
			// Tell the world that the player has missed
			GameCore.PlayerMissed();
			
			// Destroy this object
			Destroy(this.gameObject);
		}
	}
}

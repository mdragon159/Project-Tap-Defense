using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class PlayerMissile : MonoBehaviour {
	// For easy access to the player's missile
	public static PlayerMissile Instance = null;
	
	bool active = false; // Flag for activating the motion animation
	bool hit = false; // Used to stop the selfDestruct and such
	float fireSpeed = 75f; // What speed should this missile move forward?
	
		// Timer to check if 
	float waitTime = 1f; // Amount of time before declaring miss
	float startTime = 0f; // Time since fired the missile
	
	Animator animator; // Stores the animator component
	
	// Set the instance to this missile
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		animator = this.gameObject.GetComponent<Animator>();
		if(animator == null)
		{
			Debug.LogError("Cant find the animator!");
		}
		
		// Do an initial reset
		Reset();
	}
	
	void Update()
	{
		// Check if active...
		if(active)
		{
			// Check if the timer has been exceeded
			if(Time.time > startTime)
			{
				// Tell the world that the player has missed
				GameCore.PlayerMissed();
				
				// Reset this object
				Reset();
			}
		}
	}
	
	void FixedUpdate()
	{
		// If this is in motion, move it forward
		if(active)
		{
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
			
			// Destroy the enemy (enemy signals that player can attack again)
			collider.gameObject.GetComponent<EnemySpawn>().Attacked();
			
			// Activate missile on-hit stuff
			OnHit();
		}
	}
	
	// Do stuff when hit something
	void OnHit()
	{
		// Flag that this object should stop moving
		active = false;
		
		// Flag that this object has hit an enemy
		hit = true;
		
		// Make sure the missile can't destroy more than one enemy
		this.GetComponent<BoxCollider2D>().enabled = false;
		
		// Create an explosion here
		MiscSpawner.Instance.CreateExplosion0(this.transform.position, this.renderer.bounds.size.y);
		
		// Reset the missile
		Reset();
	}
	
	/*
	
	// Destroy self in certain amount of time, just in case
	IEnumerator SelfDestruct(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		// Don't self destruct if already hit an enemy
		if(!hit)
		{
			// Tell the world that the player has missed
			GameCore.PlayerMissed();
			
			// Reset this object
			Reset();
		}
	}
	
	*/
	
	// Officially deactivating/resetting the missile
	void Reset()
	{
		// Reset the parent to the player
		transform.parent = PlayerControl.playerTransform;
		
		// Reset the hit status
		hit = false;
		
		// Reset to the idle animation
		animator.SetTrigger("Reset");
		
		// Deactivate collider
		this.GetComponent<BoxCollider2D>().enabled = false;
		// Hide the missile from view
		this.renderer.enabled = false;
		
		// Deactivate officially
		active = false;
	}
	
	// Activate and fire the missile
	public void Fire(Vector3 touchPos)
	{
		// Unparent, for organizational reasons
		transform.parent = null;
	
		// Officially activate the missile
		active = true;
		
		// Make sure the missile can hit enemies again
		this.GetComponent<BoxCollider2D>().enabled = true;
		// Make the missile visible
		this.renderer.enabled = true;
		
		// Activate the motion animation
		animator.SetTrigger("Shoot");
		
		// Set the timer for this missle
		startTime = Time.time + waitTime;
		
		// Reset the position of the missile back to the player's position
			// TEMPORARY: Want to fire from barrel, technically
		this.transform.position = PlayerControl.playerTransform.position;
		
			// For setting the angle of the missile
		// Convert screen to world coordinates
		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(touchPos);
		// Get the direction
		Vector3 direction = worldMousePosition - this.transform.position;
		// Calculate the angle for that direction
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		// Angle the missile towards the pos finally
		this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}

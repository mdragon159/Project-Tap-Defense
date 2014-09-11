using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class SpawnObject : MonoBehaviour {
	
	public enum ObjectType{
		None,
		Fire0,
		Explosion0
	}
	
	public ObjectType thisType;
	
	// The parent spawnPool, so spawn can return to its spawnPool
	public Transform spawnPool;
	
	// Holds the starting size (for explosion0 currently)
	public float startSize;
	
	// If boxCollider exists, this will be enabled and disabled
	BoxCollider2D boxCollider;
	// Animator for switching states
	Animator animator;
	
	// For projectiles: determines if moving or not
	bool active = false;
	// For projectiles: determines the flight speed
	float fireSpeed = 15f;

	// Use this for initialization
	void Start () {
		// If mistakes were made, give an error
		if(thisType == ObjectType.None)
		{
			Debug.LogError("<SpawnObject> Object type is none?");
		}
	
		// Find the collider of this object, if it exists
		boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
	
		// Deactivate the spawned object
		this.gameObject.renderer.enabled = false;
		if(boxCollider != null)
		{
			boxCollider.enabled = false;
		}
		
		// Starting variables particularly for Explosion Type0
		if(thisType == ObjectType.Explosion0)
		{
			// Get the animator
			animator = this.gameObject.GetComponent<Animator>();
		
			// Starting size for calculating size of explosion
			startSize = renderer.bounds.size.x;
		}
		// Starting variables particularly for Fire0
		else if(thisType == ObjectType.Fire0)
		{
			// Get the animator
			animator = this.gameObject.GetComponent<Animator>();
		}
	}
	
	// Currently only for projectiles...
	void FixedUpdate()
	{
		if(active)
		{
			// Move the object forward
			rigidbody2D.velocity = transform.right * fireSpeed;
		}
	}
	
	public void KillSpawn()
	{
		// Deactivate the spawned object
		this.gameObject.renderer.enabled = false;
		if(boxCollider != null) // If there's a collider, then deactivate it
		{
			boxCollider.enabled = false;
		}
		
		// Return the object to its spawn pool
		this.transform.parent = spawnPool;
		
		// Reset according to object type
		if(thisType == null)
		{
			Debug.LogError("<SpawnObj> Object type is null!");
		}
		// If this is a Fire Type0...
		else if(thisType == ObjectType.Fire0)
		{
			// Mark inactive
			active = false;
			
			// Reset the animation
			animator.SetTrigger("Reset");
		}
		
	}
	
	
	// Activates this object completely
	public void Activate()
	{
		// Activate the spawned object
		this.gameObject.renderer.enabled = true;
		if(boxCollider != null) // If there's a collider, then activate it
		{
			boxCollider.enabled = true;
		}

		// If this is an explosion....
		if(thisType == ObjectType.Explosion0)
		{
			// Reset the animator state to the explosion animation
			animator.SetTrigger("Explode");
			
			// Wait 0.7 seconds before resetting the explosion
			StartCoroutine( DestroySelf(0.7f) );
		}
		// Else if this is an Fire0,...
		if(thisType == ObjectType.Fire0)
		{
			// Reset the animator to the flying animation
			animator.SetTrigger("Shoot");
			
			// Flag active, so that it moves
			active = true;
			
			// Wait 0.7 seconds before resetting the explosion
			StartCoroutine( DestroySelf(0.4f) );
		}
	}
	
		// More specific functions \\
	   //							\\
	// Wait to "destroy" self
	IEnumerator DestroySelf(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		KillSpawn();
	}
	
	// For projectiles... Yes, this is more specific in this case
	void OnTriggerEnter2D(Collider2D collider)
	{
		// Do following things only if hit an enemy
		if(collider.transform.name == "Player")
		{
			if(GameMaster.gmInstance.debugMode && GameMaster.gmInstance.invincibleMode)
			{
				// If in debug mode and invincible mode, ignore collsiions with player- aka RETURN
				return;
			}
		
			// Destroy the player
			collider.gameObject.GetComponent<PlayerControl>().Attacked();
			
			// Activate missile on-hit stuff
			StartCoroutine(OnHit());
		}
	}
	// Projectile: Explode and "destroy" self
	IEnumerator OnHit()
	{
		// Activate the explosion animation
		animator.SetTrigger("Hit");
		
		// Make sure missile doesn't move
		active = false;
		
		// Wait till the explosion ends
		yield return new WaitForSeconds(0.5f);
		
		// Destroy this missile for evah
		KillSpawn();
	}
}

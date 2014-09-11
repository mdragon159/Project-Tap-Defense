using UnityEngine;
using System.Collections;

public class EnemyFire : MonoBehaviour {
	public bool inMotion = true; // Flag for activating the motion animationn
	
	public float fireSpeed = 15f; // What speed should this missile move forward?
	public float selfDestructTime = 3f; // Amount of time before self destructing
	
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
		// Do following things only if hit an enemy
		if(collider.transform.name == "Player")
		{
			// Destroy the player
			collider.gameObject.GetComponent<PlayerControl>().Attacked();
			
			// Activate missile on-hit stuff
			StartCoroutine(OnHit());
		}
	}
	
	// Do stuff when hit something
	IEnumerator OnHit()
	{
		// Flag that this object should stop moving
		inMotion = false;
		// Activate the explosion animation
		animator.SetTrigger("Hit");
		
		// Wait till the explosion ends
		yield return new WaitForSeconds(0.5f);
		
		// Destroy this missile for evah
		Destroy(this.gameObject);
	}
	
	// Destroy self in certain amount of time, just in case
	IEnumerator SelfDestruct(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		Debug.Log("<EnemyFire> Missed the player...?");
		Destroy(this.gameObject);
	}
}

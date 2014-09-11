using UnityEngine;
using System.Collections;

public class FirstEnemy : MonoBehaviour {

	public GameObject enemyMissile;
	
	// Time until enemy gets ready to attack the player
	public float timeUntilAttack = 2f;
	// Size multiplier
	public float sizeMultiplier = 1f;
	
	// Holds the beginning localScale of Enemy0
	Vector3 standardSize;
	
	// Time until enemy self destructs after attacking
	float selfDestructTime = 2f;
	// Flag to stop from attacking more than once, to stop moving, etc
	bool attacked = false;
	// Flag to indicate if active or not
	bool active = false;

	// Save the position of the player, in order to target it
	Transform player;
	// Cache the SpawnObjectController for speed reasons - used to Kill self
	EnemySpawn spawnController;

	// Use this for initialization
	void Start () {		
	
		// Get this spawn's SpawnObjectController
		spawnController = gameObject.GetComponent<EnemySpawn>();
		
		// Cache the player's Transform for targeting
		GameObject player_go = GameObject.Find ("Player");
		player = player_go.transform;
		
		// Deactivate collider and renderer
		gameObject.renderer.enabled = false;
		
		// Sets the standard size
		standardSize = transform.localScale;
	}
	
	void Update()
	{

#if UNITY_EDITOR
/*
		if(Input.GetKeyDown("f"))
		{
			Debug.Log ("Enemy fired at the player!");
			AttackPlayer();
		}
*/
#endif

		// If attacked, zoom off screen
		if(active && transform.position.x == player.position.x)
		{	
			if(!attacked)
			{
				// Attack the player
				AttackPlayer();
			}
		}
	}
	
	void PrintEnemy0Stats()
	{
		print("<--- Enemy0 Stats --->");
		print("Enemy TUAttack: " + timeUntilAttack);
		print("<- End Stats ->");
	}
	
	IEnumerator Move(float startPosX, float endPosX, float time){
		float i = 0.0f;
		float rate = 1.0f/time;
		while (i < 1.0){
			yield return null;
			
			// If not active, exit out of this whole shenanigan!
			if(!active) i = 5f;
			
			i = i + (Time.deltaTime * rate);
			Vector2 position = this.transform.position;
			position.x = Mathf.Lerp(startPosX, endPosX, i);
			this.transform.position = position;
		}
	}
	
	void AttackPlayer()
	{	
		// Indicate that the player has been attacked
		attacked = true;
		
		// "Create" and fire the missile
		MiscSpawner.Instance.Fire0(this.transform.position);
		
		/*
		GameObject missile = (GameObject)Instantiate(enemyMissile);
		// Set the missile's position
		missile.transform.position = this.transform.position;
		
		// For setting the angle of the missile
		// Get the direction - from the missile to the player
		Vector3 direction = player.position - missile.transform.position;
		// Calculate the angle for that direction
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //* Mathf.Atan2;
		// Angle the missile towards the mouse pos
		missile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		*/
		
		StartCoroutine( FinalMovement() );
	}
	
	IEnumerator FinalMovement()
	{
		// Wait a tiny bit for "shooting while standing" effect
		yield return new WaitForSeconds(0.25f);
		
		float i = 0f;
		// How long to move for
		float time = 1f;
		float rate = 1.0f/time;
		
		// How far to move in that time
		// To calculate rate, get change in position (WidestPt-ZeroPt = Displacement)
		float zoomRate = Camera.main.ScreenToWorldPoint( new Vector3( Screen.width, 0, 0) ).x;
		zoomRate -= Camera.main.ScreenToWorldPoint( Vector3.zero).x;
		while (i < 1.0)
		{
			yield return new WaitForEndOfFrame();
			
			// If not active, exit out of this whole shenanigan!
			if(!active) i = 5;
			
			i = i + (Time.deltaTime * rate);
			
			// Zoom off the screen
			Vector2 pos = transform.position;
			pos.x += -zoomRate*Time.deltaTime;
			transform.position = pos;
		}
		
		// Finally "destroy" self and return to pool
		spawnController.KillSpawn();
	}
	
	// Activate this enemy
	public void Activate()
	{
		// Make sure object KNOWS it's active
		active = true;
		// Make sure can attack
		attacked = false;
		
		// Set the size of this enemy
		transform.localScale = standardSize*sizeMultiplier;
		
		// Move to position to attack player
		StartCoroutine( Move(this.transform.position.x, player.position.x, timeUntilAttack) );	
		
		// DEBUG/TEMPORARY
	//	PrintEnemy0Stats();
	}
	
	// Deactivate certain aspects of this script
	public void Deactivate()
	{
		// Make sure the script knows it's not active
		active = false;
	}
}

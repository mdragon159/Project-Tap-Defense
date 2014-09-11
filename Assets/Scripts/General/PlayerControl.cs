using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class PlayerControl : MonoBehaviour {

	// For easy access to the player
	public static PlayerControl Instance = null;

	// For easy access to the player's position
	public static Transform playerTransform;
	
	void Awake()
	{
		// Set the instance of this player so others can access it easily
		Instance = this;
		
		// Allow the world to access the player's transform
		playerTransform = this.transform;
	}
	
	void Start()
	{
		// Just reset the player
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(GameCore.debugCheck)
		{
			// Instant reload
			if(Input.GetKeyDown("r"))
			{
				GameCore.SetFireReady(true);
			}
		}
		#endif
	}
	
	// Reset position and more for a new game
	public void Reset()
	{
		// Make sure not dead yet
			//dead = false;
			// Now the GameCore sets that 
		
		// Reset the position
		Vector2 pos;
		pos.x = Camera.main.ScreenToWorldPoint( new Vector3( Screen.width/16, 0, 0)).x + renderer.bounds.size.x/2;;
		pos.y = Camera.main.ScreenToWorldPoint( new Vector3( 0, Screen.height*2/18, 0)).y + renderer.bounds.size.y/2;
		transform.position = pos;
		
		// Make sure NOT visible with inactive collider
		this.gameObject.renderer.enabled = false;
		this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
	}
	
	public void Activate()
	{
		// Make sure visible with active collider
		this.gameObject.renderer.enabled = true;
		this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
	}
	
	// Orient and fire the missile
	public void FireMissile(Vector3 touchPos)
	{	
		if(GameCore.playerDead) return;
		
		// Play the missile launching sound
		MusicPlayer.Instance.PlayMissileLaunch();
		
		// Mark that the missile has been fired
		GameCore.SetFireReady(false);
		
		// Fire the missile
		PlayerMissile.Instance.Fire(touchPos);
		
		/*
		// Create the missile
		GameObject missile = (GameObject)Instantiate(missilePrefab);
		// Set the missile's position
		missile.transform.position = this.transform.position;
		
			// For setting the angle of the missile
		// Convert screen to world coordinates
		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(touchPos);
		// Get the direction
		Vector3 direction = worldMousePosition - missile.transform.position;
		// Calculate the angle for that direction
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		// Angle the missile towards the pos finally
		missile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		*/
	}
	
	// What happens when attacked by the enemy
	public void Attacked()
	{	
		// If not already dead, do the rest of the code
		if(GameCore.playerDead) return;
		
		// Indicate player is dead
		GameCore.SetDeathState(true);
		
		// Play the death explosion SFX
		MusicPlayer.Instance.PlayExplosion02();
		// Broadcast that the player died
		GameMaster.gmInstance.DelayedGameOver(1.15f);
		
		// Hide this character
		this.gameObject.renderer.enabled = false;
		// Disable the collider so no addiitonal impacts
		this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		
		// Create an explosion in the player's place
		MiscSpawner.Instance.CreateExplosion0(this.transform.position, this.renderer.bounds.size.y);
		
	}
}

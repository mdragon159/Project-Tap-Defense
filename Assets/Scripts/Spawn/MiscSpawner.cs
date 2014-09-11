using UnityEngine;
using System.Collections;

public class MiscSpawner : MonoBehaviour {

	public static MiscSpawner Instance = null;

	public GameObject activePool;
	public SpawnPool explosion0Pool;
	public SpawnPool fire0Pool;

	void Awake()
	{
		// Make sure the instance is set to this- there should only be ONE instance of this!
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		// If a pool wasn't attached, give an error
		if(activePool == null || explosion0Pool == null || fire0Pool == null)
		{
			Debug.LogError("<MiscSpawner> Didn't attach a pool!");
		}
	}
	
	// Resets all active objects
	public void ResetAllObjects()
	{
		SpawnObject[] spawns = activePool.GetComponentsInChildren<SpawnObject>();
		//	Debug.Log ("Number of spawns in spawns: " + spawns.Length);
		
		// Disable every enemy, one by one
		foreach (SpawnObject obj in spawns)
		{
			obj.KillSpawn();
		}
	}
	
	// Create an Explosion Type0 anywhere for "any" size
	public void CreateExplosion0(Vector3 pos, float sizeMultiplier)
	{
		// Get an explosion
		GameObject explosion0 = explosion0Pool.GetSpawn();
		
		// Set the position of the explosion
		explosion0.transform.position = pos;
		
		// Set the size of the explosion, if requested
		if(sizeMultiplier > 0) 
		{
			// Get the current size, for setting the explosion exactly
			float startSize = explosion0.GetComponent<SpawnObject>().startSize;
			// Finally set the size of the explosion
			explosion0.transform.localScale = new Vector2 (sizeMultiplier/startSize, sizeMultiplier/startSize);
		}
		
		// Activate the explosion
		explosion0.GetComponent<SpawnObject>().Activate();
	}
	
	// Create a EnemyFire0 that shoots directly at the player
	public void Fire0(Vector3 startPos)
	{
		GameObject fire0 = fire0Pool.GetSpawn();
		
		// Set the missile's position
		fire0.transform.position = startPos;
		
		// For setting the angle of the missile
		// Get the direction - from the missile to the player
		Vector3 direction = PlayerControl.playerTransform.position - fire0.transform.position;
		// Calculate the angle for that direction
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //* Mathf.Atan2;
		// Angle the missile towards the mouse pos
		fire0.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		// Active the missile
		fire0.GetComponent<SpawnObject>().Activate();
	}
}

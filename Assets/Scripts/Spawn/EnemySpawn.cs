using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class EnemySpawn : MonoBehaviour {
	
	// Determines the behaviour of this enemy
	public SpawnCore.EnemyType thisEnemy;
	
	// The parent spawnPool, so spawn can return to its spawnPool
	public Transform spawnPool;

	// Use this for initialization
	void Start () {
		// Deactivate the spawned object
		this.gameObject.renderer.enabled = false;
		this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Attacked()
	{
		// Allow player to fire again
		GameCore.SetFireReady(true);
		// Increase the game's score by one
		GameCore.ChangeScore(1);
		
		// Play the explosion SFX
		MusicPlayer.Instance.PlayExplosion01();
		
		// Set an explosion in this enemy's place
		MiscSpawner.Instance.CreateExplosion0(this.transform.position, this.renderer.bounds.size.y);
		
		// Reset this enemy and return it to its spawn pool
		KillSpawn();
	}
	
	public void KillSpawn()
	{
		// Broadcast that an enemy has been destroyed this wave
		SpawnCore.IncrementWaveDestroyed();
	
		// Deactivate the spawned object
		this.gameObject.renderer.enabled = false;
		this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
		
		// Return the object to its spawn pool
		this.transform.parent = spawnPool;
		
		// Reset according to enemy type
		// Activate movement/attack depending on enemy type
		if(thisEnemy == null)
		{
			Debug.LogError("<SpawnObj> Object type is null!");
		}
		if(thisEnemy == SpawnCore.EnemyType.Enemy0)
		{
			this.gameObject.GetComponent<FirstEnemy>().Deactivate();
		}
	}
	
	// Activates this object completely
	public void Activate()
	{
		// Activate the spawned object
		this.gameObject.renderer.enabled = true;
		this.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
		
		// Activate movement/attack depending on enemy type
		if(thisEnemy == null)
		{
			Debug.LogError("<SpawnObj> Object type is null!");
		}
		if(thisEnemy == SpawnCore.EnemyType.Enemy0)
		{
			this.gameObject.GetComponent<FirstEnemy>().Activate();
		}
	}
}

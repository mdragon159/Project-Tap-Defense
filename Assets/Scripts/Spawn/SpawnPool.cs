using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class SpawnPool : MonoBehaviour {

	// Used for setting the parent of the enemies
	public Transform activePool;
	public GameObject objectPrefab;
	public int initialSpawns = 15;
	public bool useAdditional = true; // Flags whether or not to create more objects when low
	public int additionalSpawns = 5;
	
	// Use this for initialization
	void Start () {
		if(activePool == null)
		{
			GameObject activePool_go = GameObject.Find ("_ActivePool");
			activePool = activePool_go.transform;
			
		//	activePool = GameObject.Find ("_ActivePool");
			Debug.LogError("<SpawnMaster> ActivePool wasn't attached.");
		}
	
		CreateSpawns(initialSpawns);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CreateSpawns(int numOfCopies)
	{
		// For how many copies need to be made...
		for(int i = 0; i < numOfCopies; i++)
		{
			// Make an instance of that enemy
			GameObject obj = (GameObject)Instantiate(objectPrefab);
			// Set it under this SpawnPool for easy access
			obj.transform.parent = this.transform;
			
			// Set the object's spawn pool to this, so it can return
			SpawnObject objSpawnController = obj.GetComponent<SpawnObject>();
			// If the objSpawnController doesn't exit, then this is an enemy spawn
			if(objSpawnController != null)
			{
				objSpawnController.spawnPool = this.gameObject.transform;
			}
			else
			{
				EnemySpawn enemySpawnController = obj.GetComponent<EnemySpawn>();
				enemySpawnController.spawnPool = this.gameObject.transform;
			}
		}
	}
	
	public GameObject GetSpawn()
	{	
		// If there are no more children to access
		if(transform.GetChild(0).gameObject == null)
		{
			Debug.Log ("<SpawnPool> Returned null of " + objectPrefab.name);
			return null;
		}
	
		// The spawnable to return is simply the first child - for simplicity
		GameObject spawnable = transform.GetChild(0).gameObject;
		// Make sure this is now unparented, for sorting reasons
		spawnable.transform.parent = activePool;
		
		// If no more spawns under this pool, then create some supplementary ones
		if(transform.childCount < 1)
		{
			// If we can make more...
			if(useAdditional)
			{
				CreateSpawns(additionalSpawns);
			}
		}
		
		return spawnable;
	}
}

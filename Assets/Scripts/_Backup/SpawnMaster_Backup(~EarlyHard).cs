/*
using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class SpawnMaster : MonoBehaviour {

	public static SpawnMaster Instance = null;

	public GameObject activePool;
	public SpawnPool spawnPool0;
	
	bool active = false; // Determines whether this is active or not
	bool spawnCoolDown = false; // Determines whether can spawn again or not
	
		// Wave settings
	float waveStartTime = 0f;
	// Time to wait between each wave
	float waveWaitTime = 0f;
	
	// Just for benchmarking the easy and hard between-spawn delay times
	float hardBenchmark = 0.25f - 0.1f;
	float easyBenchmark = 1.42f + 0.03f;
	
		// TEMPORARY DEBUG VARIABLES
	float waitTime2 = 1.2f;
	float lastSpawnTime = 0f;
	
	void Awake()
	{
		// Make sure Instance is set to this	
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		// Get the spawn pool(s) if not attached
		if(activePool == null)
		{
		//	GameObject activePool_go = GameObject.Find ("_ActivePool");
		//	activePool = activePool_go.transform;
		
			activePool = GameObject.Find ("_ActivePool");
			Debug.LogError("<SpawnMaster> ActivePool wasn't attached.");
		}
		if(spawnPool0 == null)
		{
			GameObject SpawnPool0_go = GameObject.Find ("SpawnPool0");
			spawnPool0 = SpawnPool0_go.GetComponent<SpawnPool>();
			Debug.LogError("<SpawnMaster> SpawnPool0 wasn't attached.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		// Spawn an enemey if e key is pressed
		if(Input.GetKeyDown ("e"))
		{
			// If not active, don't continue
			if(!active)
			{
				print ("<Spawner> Spawner inactive");
				return;
			}
			
				// FOR GETTING TIMING BETWEEN SPAWNING ENEMIES
			if(lastSpawnTime == 0f)
			{
				print("Setting first spawn time");
				
				// Set initial lastSpawnTime
				lastSpawnTime = Time.time;
			}
			else
			{
				print("Time Between Spawns: " + (Time.time - lastSpawnTime) );
				
				// Reassign lastSpawnTime
				lastSpawnTime = Time.time;
			}
			
			// Increase the amount spawned
			SpawnCore.IncreaseSpawnTotals();
			
			// Actually spawn the enemy
			SpawnEnemy0( spawnPool0.GetSpawn() );
		}
		
		// Print spawn statistics
		if(Input.GetKeyDown ("s"))
		{
			print("<---- Printing Spawn Statistics ---->");
			
			print("Total Spawned: " + SpawnCore.totalSpawned);
			print("Current Wave: " + SpawnCore.currentWave);
			print("This Wave's Goal: " + SpawnCore.thisWaveGoal);
			print("This Wave's Spawned: " + SpawnCore.thisWaveSpawns);
			print("This Wave's Destroyed: " + SpawnCore.thisWaveDestroyed);
			
			print("<-- End Spawn Statistics -->");
		}
		#endif
		
		// If the Spawner is active...
		if(active)
		{			
			// If not enough spawns have been made to reach this spawn's goals
			if(SpawnCore.thisWaveSpawns < SpawnCore.thisWaveGoal)
			{		
				// If spawn is NOT on cool down, then continue spawning
				if(!spawnCoolDown)
				{
					float waitTime = Random.Range (0.05f, 1.2f);
				//	waitTime2 -= 0.5f;
				//	print("WaitTime2: " + waitTime2);
				
					// Spawn an enemy
					StartCoroutine( DelayedSpawn(SpawnCore.EnemyType.Enemy0, 0.1f) );
				}
			} // end thisWaveGoal != thisWaveSpawn
			
			// Else check that the number destroyed == thisWaveGoal
			else if(SpawnCore.thisWaveGoal <= SpawnCore.thisWaveDestroyed)
			{			
				// Activate the next wave
					// GHETTO TYPE: Increase by one for both how many to spawn and wave number
				SpawnCore.NewWave (SpawnCore.currentWave+1, SpawnCore.thisWaveGoal+1);
				
			//	print("Wave Goal: " + SpawnCore.thisWaveGoal);
			//	print("Current Wave: " + SpawnCore.currentWave);
			}
		}
	
	}
	
	IEnumerator DelayedSpawn(SpawnCore.EnemyType enemyType, float waitTime)
	{
		
		// Don't let spawning back to back continuously
		spawnCoolDown = true;
		
		// Spawn the enemy according to the wishes from above
		if(enemyType == SpawnCore.EnemyType.Enemy0)
		{
			SpawnEnemy0( spawnPool0.GetSpawn() );
		}
		else
		{
			Debug.LogError ("<Spawner> Stupid idiot, that enemy type don't exist");
		}
		
		// Increment stats
		SpawnCore.IncreaseSpawnTotals();
		
		// Wait until allowing to spawn again
		yield return new WaitForSeconds(waitTime);
		// THEN allow spawning again
		spawnCoolDown = false;
	}
	
	// Actually spawn the enemy
	void SpawnEnemy0(GameObject enemyPrefab)
	{
		GameObject enemy = enemyPrefab;
		// TEMPORARY CODE: Make more generalized later
		FirstEnemy enemyScript = enemy.GetComponent<FirstEnemy>();
		
		// Set the time until attack for this enemy
		enemyScript.timeUntilAttack = Random.Range (0.95f, 2f);
	//	enemyScript.timeUntilAttack = 0.95f;
		
		// Set the size of the enemys
		enemyScript.sizeMultiplier = Random.Range (0.3f, 1.2f);
	//	enemyScript.sizeMultiplier = 0.3f;
		
		// Set the position of this enemy
		Vector2 pos = Vector2.zero;
		// Set the enemy just right off the screen
		pos.x = Camera.main.ScreenToWorldPoint( new Vector3( Screen.width, 0, 0) ).x + enemy.renderer.bounds.size.x/2;
		
		// Calculate the max height for spawning- just at the top of the screen
		float maxHeight = Camera.main.ScreenToWorldPoint( new Vector3( 0, Screen.height, 0) ).y - enemy.renderer.bounds.size.y/2;
		// Calculate the min height for spawning- 3/5s of the screen- right above the layer
		float minHeight = Camera.main.ScreenToWorldPoint( new Vector3( 0, Screen.height*3/5, 0) ).y - enemy.renderer.bounds.size.y/2;
		// Set the enemy to a random height between the max and min heights
		pos.y = Random.Range(minHeight, maxHeight);
		
		// Apply the position to the enemy
		enemy.transform.position = pos;
		
		// Activate the object
		enemy.GetComponent<EnemySpawn>().Activate();
		// TEMPORARY: Make this part MORE general somehow- probably with EnemySpawnController
		enemy.GetComponent<FirstEnemy>().Activate();
	}
	
	public void ResetAllEnemies()
	{
		EnemySpawn[] spawns = activePool.GetComponentsInChildren<EnemySpawn>();
	//	Debug.Log ("Number of spawns in spawns: " + spawns.Length);
		
		// Disable every enemy, one by one
		foreach (EnemySpawn enemy in spawns)
		{
			enemy.KillSpawn();
		}
	}
	
	public void StartSpawning()
	{
		// Set the spawner to active
		active = true;
		spawnCoolDown = false;
		
		// Start the wave
		SpawnCore.NewWave(1, 1);
	}
	
	public void StopSpawning()
	{
		// Stop the spawner from activating
		active = false;
	}
}
*/

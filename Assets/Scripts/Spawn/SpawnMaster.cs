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
			print("This Wave's Goal: " + SpawnCore.thisWave_goal);
			print("This Wave's Spawned: " + SpawnCore.thisWave_spawned);
			print("This Wave's Destroyed: " + SpawnCore.thisWave_defeated);
			
			print("<-- End Spawn Statistics -->");
		}
		#endif
		
		// If the Spawner is active...
		if(active)
		{			
			// If not enough spawns have been made to reach this spawn's goals
			if(SpawnCore.thisWave_defeated < SpawnCore.thisWave_goal)
			{		
				// If spawn is NOT on cool down, then continue spawning
				if(!spawnCoolDown)
				{
					// Spawn an enemy
					StartCoroutine( DelayedSpawn(SpawnCore.EnemyType.Enemy0) );
				}
			} // end thisWaveGoal != thisWaveSpawn
			
			// Else check that the number destroyed == thisWaveGoal
			else if(SpawnCore.thisWave_goal <= SpawnCore.thisWave_defeated)
			{			
				// Activate the next wave
				SpawnCore.NextWave();
				
				print("Current Wave: " + SpawnCore.currentWave);
			}
		}
	
	}
	
	IEnumerator DelayedSpawn(SpawnCore.EnemyType enemyType)
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
		yield return new WaitForSeconds(SpawnCore.spawnDelay);
		
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
		enemyScript.timeUntilAttack = Random.Range (SpawnCore.attackDelay_Min, SpawnCore.attackDelay_Max);
	//	enemyScript.timeUntilAttack = 0.95f;
		
		// Set the size of the enemys
		float sizeMultiplier = Random.Range (SpawnCore.sizeMultiplier_Min, SpawnCore.sizeMultiplier_Max);
		enemyScript.sizeMultiplier = sizeMultiplier;
		
		// Set the position of this enemy
		Vector3 pos = Vector2.zero;
		// Set the enemy just right off the screen
		pos.x = Camera.main.ScreenToWorldPoint( new Vector3( Screen.width, 0, 0) ).x + enemy.renderer.bounds.size.x/2;
		// Z-sort so larger enemies are behind smaller ones
		pos.z = sizeMultiplier * 5;
		
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
		
		// Set up the first wave
		SpawnCore.FirstWave();
	}
	
	public void StopSpawning()
	{
		// Stop the spawner from activating
		active = false;
	}
}

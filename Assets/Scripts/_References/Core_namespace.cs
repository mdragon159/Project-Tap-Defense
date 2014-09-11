using UnityEngine;
using System.Collections;

namespace com.dTT.Core
{
	// Class contains all the variables and methods necessary for the core of the game
	public static class GameCore
	{
		// Determines state of the game
		public enum GameState{
			None,
			Start,
			InGame,
			GameOver
		}
		
		// Holds the state of the game
		public static GameState gameState;
	
		// Static, easily accessible variables
		public static int curScore = 0; // Keeps the score for the game
		public static int highScore = 0; // Holds the high score
		public static bool debugCheck = false; // Used to broadcast if in debug mode
		public static bool fireReady = true; // Flag for firing missile
		public static bool gameInProgress = true; // Flag for stating that the game is in progress
		public static bool playerMissed = false; // Flag for indicating that the player has missed the enemy
		public static bool playerDead = false; // Flag for player alive/dead status
		public static bool pauseState = false; // Flag for indicating whether or not the game is paused
		
		// Just resets own static variables
		static public void ResetGameVars()
		{
			curScore = 0;
			fireReady = true;
			
			// Make the following false in the near future, since there'll be a start button
			gameInProgress = true;
			
			playerMissed = false;
			playerDead = false;
			pauseState = false;
		}
		
		// Set debug mode
		static public void SetDebugCheck(bool state)
		{
			debugCheck = state;
		}
		
		// Change the gameState
		static public void SetGameState(GameCore.GameState setState)
		{
			GameCore.gameState = setState;
		}
		
		// Change the state of the player's life
		static public void SetDeathState(bool state)
		{
			playerDead = state;
		}
		
		// Get the high score
		static public void GetHighScore()
		{
			highScore = PlayerPrefs.GetInt("HighScore");
		}
		
		// Check if new high score and set the high score
		static public void NewScore()
		{
			// If the current score is higher then the previous high score...
			if(curScore > highScore)
			{
				// Set the high score
				PlayerPrefs.SetInt("HighScore", curScore);
				
				// Set the new high score
				highScore = curScore;
				
				// Save the change
				PlayerPrefs.Save();
			}
			
			return;
		}
		
		// Instantly change fireReady
		static public void SetFireReady(bool state)
		{
			// Set the state of fireReady
			fireReady = state;
		}
		// Adds a delay to resetting fireReady
		static public IEnumerator SetFireReady(float waitTime, bool state)
		{
			// Wait the right amount of time
			yield return new WaitForSeconds(waitTime);
			
			// Set the state of fireReady
			fireReady = state;
		}
		
		// Increment the game's score
		static public void ChangeScore(int change)
		{
			curScore += change;
		}
		// Just in case for absolute setting the game's score
		static public void SetScore(int set)
		{
			curScore = set;
		}
		
		// Tell the game that the player is a failure and missed the enemy
		static public void PlayerMissed()
		{
			Debug.Log ("Player is a failure, and should now be demolished by enemies!");
			
			// Change the status of this bool... maybe useless?
			playerMissed = true;
			
			// Tell WaveSpawner to stop spawning enemies
			// INSERT CODE HERE!
			
			// Somehow broad cast to the enemies that the player missed, to start the death 
			/// INSERT CODE HERE! Maybe: 1) Find all enemies tagged with this, 2) Pick a random one, 3) Tell it to do FinalAttack()?
		}
		
	}	 // Close GameCore class
	
	// Class contains all the necessary global variables and functions
	public static class SpawnCore
	{
		public enum EnemyType{
			None,
			Enemy0,
			Enemy1
		}
	
		// The number of enemies spawned so far this game
		public static int totalSpawned = 0;
		// The wave number
		public static int currentWave = 0;
		
		// The number of spawns to be sent out this wave
		public static int thisWave_goal = 0;
		// The number of spawns actually spawned this wave
		public static int thisWave_spawned = 0;
		// The number of spawns destroyed this wave
		public static int thisWave_defeated = 0;
		
		// Determine the delay between individual spawns
		public static float spawnDelay = 0f;
		
		// Determine the max and min time until attack
		public static float attackDelay_Max = 0f;
		public static float attackDelay_Min = 0f;
		
		// Determine the max and min size multiplier
		public static float sizeMultiplier_Max = 0f;
		public static float sizeMultiplier_Min = 0f;
		
		// Determines the delay time berween whole waves
		public static float waveDelay = 1.5f;
		
		// Resets all the spawned variables for a new game
		public static void ResetSpawnVars()
		{
			totalSpawned = 0;
			currentWave = 0;
			thisWave_goal = 0;
			thisWave_spawned = 0;
			thisWave_defeated = 0;
		}
		
		// Set the new wave
		public static void OLDNewWave(int waveNumber, int waveGoal)
		{
			// Set the wave-dependent variables
			thisWave_goal = waveGoal;
			thisWave_spawned = 0;
			thisWave_defeated = 0;
			
			// Set the wave number
			currentWave = waveNumber;
		}
		
		// Set the first wave up
		public static void FirstWave()
		{
				//> Reset spawned and defeated numbers just in case
			thisWave_spawned = 0;
			thisWave_defeated = 0;
			
				//> On the first wave
			currentWave = 1;
			
				//> Set all the wave default values
			thisWave_goal = 2; // Super easy beginning
			
			spawnDelay = 1.2f;
			
			attackDelay_Max = 2f;
			attackDelay_Min = 2f;
			
			sizeMultiplier_Max = 1.2f;
			sizeMultiplier_Min = 1.2f;
		}
		
		// Set the next wave up
		public static void NextWave()
		{
				//> Reset the number of enemies spawned and defeated
			thisWave_spawned = 0;
			thisWave_defeated = 0;
		
				//> Increment the current wave
			currentWave++;
			
				//> Set the new goal for this wave
		//	thisWave_goal = 2 + 5*(currentWave-1);
			if(currentWave <= 4) // Waves 1~4
			{
				thisWave_goal = 2 + 2*(currentWave-1);
			}
			else // Waves 5~inf.
			{
				thisWave_goal = 3 + 4*(currentWave-1); 
			}
			
				//> Set the delay between individual spawns
			// From waves 1-6, use this equation
			if(currentWave <= 6)
			{
				spawnDelay = 1.2f - 0.2f*(currentWave-1);
			}
			// Else, use the max value
			else
			{
				spawnDelay = 0.05f;
			}
			
				//> Set the max and min time until attack [Enemy0]
			if(currentWave <= 9) // From waves 1~9
			{
				attackDelay_Max = 2f - 0.1f*(currentWave-1);
			}
			else
			{
				// For craziness, the max is super slow for confusion
				attackDelay_Max = 2f;
			}
			
			if(currentWave <= 6) // From waves 2~6
			{
				attackDelay_Min = 1.7f - 0.2f*(currentWave-2);
			}
			else
			{
				// The fastest attack time, for sanity's sake, is 0.95 seconds
				attackDelay_Min = 0.95f;
			}
						
				//> Set the max and min size multiplier
			if(currentWave <= 7) // From waves 1~7
			{
				sizeMultiplier_Max = 1.2f - 0.1f*(currentWave-1);
			}
			else
			{
				// For craziness, the max is super big for confusion and blocking shots
				sizeMultiplier_Max = 1.2f;
			}
			
			if(currentWave <= 7) // From waves 1~5
			{
				sizeMultiplier_Min = 1.2f - 0.2f*(currentWave-1);
			}
			else
			{
				// Super tiny guys- any smaller is impossible at this stage
				sizeMultiplier_Min = 0.3f;
			}
			
		} // End NextWave()
		
		// Increase amount spawned so far by one
		public static void IncreaseSpawnTotals()
		{
			totalSpawned++;
			thisWave_spawned++;
		}
		
		// Increase amount spawned so far by a number
		public static void IncreaseSpawnTotals(int increase)
		{
			totalSpawned += increase;
			thisWave_spawned += increase;
		}
		
		// Increment amount destroyed so far for this round
		public static void IncrementWaveDestroyed()
		{
			thisWave_defeated++;
		}
	}
} // Close com.dTT.Core namespace
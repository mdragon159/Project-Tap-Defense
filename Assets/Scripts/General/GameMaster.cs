using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class GameMaster : MonoBehaviour {

	public static GameMaster gmInstance = null;
	public bool debugMode = false; // Used for toggling debug mode
	public bool invincibleMode = false; // Used for making invincible if above is true
	public bool playerDead = false; // Used for the gameOver screen... so far
	
	// Timers
	float gameOverWaitTime = 1.15f; // Unused- playerControl determines this (AKA temp. and for looks)
	float gameOverStartTime = 0f; // Used for determining when to switch to the gameOver screen
	
	// GUI Controllers
	public GameObject InGameGUI;
	public GameObject StartGUI;
	public GameObject GameOverGUI;
	
	void Awake()
	{
		// Make sure gmInstance is set to this	
		gmInstance = this;
	}
	
	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		GameCore.SetDebugCheck(debugMode);
		#endif
		
		// Find the GUI Controllers if not attached
		if (InGameGUI == null)
		{
			Debug.LogError("<GM> InGameGUI was not initially attached");
			InGameGUI = GameObject.Find ("InGameGUI");
		}
		if (StartGUI == null)
		{
			Debug.LogError("<GM> StartGUI was not initially attached");
			StartGUI = GameObject.Find ("StartGUI");
		}
		if (GameOverGUI == null)
		{
			Debug.LogError("<GM> GameOverGUI was not initially attached");
			GameOverGUI = GameObject.Find ("GameOverGUI");
		}
		
		// Set the default state of the game to the Start state
		ToggleGameState(GameCore.GameState.Start);
		// Set the default variables for SpawnCore just like the above
		SpawnCore.ResetSpawnVars();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		// Reset the enemies
		if( Input.GetKeyDown("b") )
		{
		//	SpawnMaster.Instance.ResetAllEnemies();
			RestartGame();
		}
		
		#endif
		
		// If the player is dead...
		if(GameCore.playerDead)
		{
			// Activate GameOver screen if timer is up and not yet at the GameOver screen
			if(GameCore.gameState != GameCore.GameState.GameOver && Time.time > gameOverStartTime)
			{
				ToggleGameState(GameCore.GameState.GameOver);
			}
		}
	}
	
	// Sets positions and other settings
	void SetGame()
	{
		// Set the standard settings for GameCore variables
		GameCore.ResetGameVars();
		// Set the default variables and standard settings for SpawnCore just like the above
		SpawnCore.ResetSpawnVars();
		
		// Tell player to reset itself
		PlayerControl.Instance.Reset ();
	}
	
	// Actually begins the game, ie start enemy spawns and allow firing
	void StartGame()
	{
		// Allow firing
		GameCore.fireReady = true;
		
		// Activate the player
		PlayerControl.Instance.Activate();
		
		// Start wave spawning
		SpawnMaster.Instance.StartSpawning();
	}
	
	// Parses the input on a clickable and activates based on game state
	public void ClickableGUI(string clickName)
	{
			// Function depends on what state the game is in	
		// If the current state of the game is at the Start
		if(GameCore.gameState == GameCore.GameState.Start)
		{
			// Send the Activation command to the StartGUI
			StartGUI.GetComponent<StartGUI>().Activate(clickName);
		}
		
		else if(GameCore.gameState == GameCore.GameState.InGame)
		{
			// Send the Activation command to the InGameGUI
			InGameGUI.GetComponent<InGameGUI>().Activate(clickName);
		}
		
		else if(GameCore.gameState == GameCore.GameState.GameOver)
		{
			// Send the Activation command to the GameOverGUI
			GameOverGUI.GetComponent<GameOverGUI>().Activate(clickName);
		}
	}
	
	// Changes the state of the game
	public void ToggleGameState(GameCore.GameState setState)
	{
		// Change the state of the game to the Start
		if(setState == GameCore.GameState.Start)
		{
			// Make the entire world recognize the game state
			GameCore.SetGameState(GameCore.GameState.Start);
			
			// Get the high score
			GameCore.GetHighScore();
			
			// Toggle the active GUI to the StartGUI
			StartGUI.SetActive(true);
			InGameGUI.SetActive(false);
			GameOverGUI.SetActive(false);
			
			// Set the game up
			SetGame();
		}
		
		// Change the state of the game to the InGame state
		else if(setState == GameCore.GameState.InGame)
		{
			// Make the entire world recognize the game state
			GameCore.SetGameState(GameCore.GameState.InGame);
			
			// Toggle the active GUI
			StartGUI.SetActive(false);
			InGameGUI.SetActive(true);
			GameOverGUI.SetActive(false);
			
			// Start the game
			StartGame();
		}
		
		// Change the state of the game to the GameOver state
		else if(setState == GameCore.GameState.GameOver)
		{
			// Make the entire world recognize the game state
			GameCore.SetGameState(GameCore.GameState.GameOver);
			
			// Check and set the new score
			GameCore.NewScore();
			
			if(GameCore.curScore > GameCore.highScore)
			{
				Debug.Log("New high score!");
			}
			
			// Toggle the active GUI
			StartGUI.SetActive(false);
			InGameGUI.SetActive(false);
			GameOverGUI.SetActive(true);
		}
	}
	
	// Used to broadcast that the game is over
	public void GameOver()
	{
		// Tell the Spawner to stop making enemies
		SpawnMaster.Instance.StopSpawning();
		
		// Tell the world the game's status in 0 seconds
		gameOverStartTime = Time.time;
	}
	
	// Does the same as GameOver, but gives a delayed effect (called from PlayerControl)
	public void DelayedGameOver(float waitTime)
	{
		// Tell the Spawner to stop immediately
		SpawnMaster.Instance.StopSpawning();
		
		// Set the timer until the gameOver screen changes
		gameOverStartTime = Time.time + waitTime;
		
		// Wait before changing the state of the game
			//yield return new WaitForSeconds(waitTime);
		
		// Tell the world the game's status
			//ToggleGameState(GameCore.GameState.GameOver);
	}
	
	// Used to restart the game, access via GameMaster.gmInstance.RestartGame()
	public void RestartGame()
	{
		// Kill all enemies
		SpawnMaster.Instance.ResetAllEnemies();
		// Reset all objects
		MiscSpawner.Instance.ResetAllObjects();
	
		// Set the state of the game at the beginning
		ToggleGameState(GameCore.GameState.Start);
	}
	
	// Tells the game to toggle pausing the game and to do anything TO (un)pause the game
	public void PauseGame()
	{
		// If already paused...
		if(GameCore.pauseState)
		{
			// Undarken the screen
			DarkenScreen.UnBlockScreen();
		
			// Indicate that not paused anymore
			GameCore.pauseState = false;
			
			// Set the game back in motion
			Time.timeScale = 1;
		}
		else // Pause the game
		{
			// Darken the screen
			DarkenScreen.BlockScreen(0.7f);
			
			// State that the game is now paused
			GameCore.pauseState = true;
			
			// Officially "pause" the game
			Time.timeScale = 0;
		}
	} // close PauseGame
} // close GameMaster

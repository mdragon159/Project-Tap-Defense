using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class GameOverGUI : MonoBehaviour {

	// On-screen Graphics
	public GameObject gameOverText;
		// Temporary: Exact position of the gameOverText
	Vector3 gameOverPos = new Vector3( 0f, 4.198024f, 0f );

	// Interactable buttons
	public GameObject restartButton;
		// Exact position of the playButton, for TEMPORARY purposes
	Vector3 restartPos = new Vector3(0.0802204f, 0.1388831f, 0f);

	// Use this for initialization
	void Start () {
		// TEMPORARY: Set the positions of the GUI
		gameOverText.transform.position = gameOverPos;
		restartButton.transform.position = restartPos;	
	
	}
	
	// TEMPORARY: Just to show the score
	void OnGUI()
	{
		// Show the current score
		GUI.skin.box.fontSize = Screen.height*7/100;
		GUI.Box(new Rect(0,0,Screen.width*9/40,Screen.height*4/40), "Score: " + GameCore.curScore);
		
		// Show the high score underneath
		GUI.skin.box.fontSize = Screen.height*5/100;
		GUI.Box(new Rect(0,Screen.height*4/40,Screen.width*9/40,Screen.height*4/40), "High Score: " + GameCore.highScore);
	}
	
	// Activates buttons based off the name of what was clicked
	public void Activate(string clickName)
	{
		// If name == restartButton's name, then we have a match!
		if(restartButton.transform.name == clickName)
		{
			// Play the general MenuClick SFX
			MusicPlayer.Instance.PlayMenuClick();
			
			// Tell the game to restart
			GameMaster.gmInstance.RestartGame();
		}
	}
}

using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class StartGUI : MonoBehaviour {

	public GameObject startButton;

	// Use this for initialization
	void Start () {
		// Find the startButton GO if it hasn't been attached
		if(startButton == null)
		{
			Debug.Log ("<GameGUI> Start Button wasn't attached");
			startButton = GameObject.Find ("StartButton");
		}
		
		// Set up the position and size of the startButton
		SetStartButton();
	}
	
	// TEMPORARY
	void OnGUI()
	{
		GUI.skin.box.fontSize = Screen.height*5/100;
		// Show the high score
		GUI.Box(new Rect(0,0,Screen.width*9/40,Screen.height*4/40), "High Score: " + GameCore.highScore);
	}
	
	void SetStartButton()
	{
		// Store the BoxCollider temporarily for the button
		BoxCollider2D thisCollider = startButton.GetComponent<BoxCollider2D>();
		
		/*\ For setting box collider
		 *  1) Store size and center
		 *  2) First calculate new position and size of the button
		 *  3) Get the ratio of new size to old size
		 *  4) Apply ratio to box collider if necessary
		\*/
	/*	
		// Holds the size multiplier for the 
		float sizeMultiplier;
		// Should be 3/5s of the screen's height
		sizeMultiplier = Camera.main.ScreenToWorldPoint( new Vector3( 0, Screen.height*3/5, 0) ).y;
		// Difference is the actual height in y coordinates (bottom of screen != 0)
		sizeMultiplier -= Camera.main.ScreenToWorldPoint( Vector3.zero ).y;
		// Start button pixel to units is 100- take that into consideration
		sizeMultiplier = sizeMultiplier*2;
		
	//	startButton.transform.localScale = Vector2.one * sizeMultiplier;
	*/
	
		// Size is hardcoded for the ghetto-temporary start screen
		startButton.transform.localScale = new Vector2(11,11);
		
		// Center the button (0,0)
		Vector3 pos = startButton.transform.position;
		pos.x = 0f;
		pos.y = 0f;
		startButton.transform.position = pos;
		
		
	}
	
	// Activates buttons based off the name of what was clicked
	public void Activate(string clickName)
	{
		// If the start button was clicked
		if(clickName == startButton.transform.name)
		{
			// Play the general MenuClick SFX
			MusicPlayer.Instance.PlayMenuClick();
			
			// Officially start the game
			GameMaster.gmInstance.ToggleGameState(GameCore.GameState.InGame);
		}
		return;
	}
}

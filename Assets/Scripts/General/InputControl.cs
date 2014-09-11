using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

/*\ Input Controls
	Interprets all input
	May double as a GUI controller, as it has to parse whether or not it clicked on a GUI, and where to send it
\*/

public class InputControl : MonoBehaviour {

	public bool unityRemote = false;
	GameObject player; // Store for calling functions
	GameMaster gameMaster; // Stores the GameMaster... which is this object- for clarity purposes
	InGameGUI inGameGUI;

	// Use this for initialization
	void Start () {
		// Find the Player
		player = GameObject.Find ("Player");
		// Find the GameMaster
		gameMaster = this.gameObject.GetComponent<GameMaster>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// If on mobile, use the following code
		#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
		
		// Check if there are any touches at all currently
		if(Input.touchCount>0)
		{
			// Do the following code for EACH touch
			for(int i = 0; i < Input.touchCount; i++){
				Touch touch = Input.GetTouch(i); // Get the current touch
				
				// Do the following code for when the touch BEGAN
				if(touch.phase == TouchPhase.Began)
				{
					Debug.Log ("Detected a SINGLE touch!");
					DoStuff(touch.position);
				} // End if touchphase.began
				
			} // End for each touch
		} // end if there are any touches
		
		#endif
		
		// If on PC, use the following code
		#if UNITY_EDITOR
		if(unityRemote) // If using unityRemote, use following code
		{
			// Check if there are any touches at all currently
			if(Input.touchCount>0)
			{
				// Do the following code for EACH touch
				for(int i = 0; i < Input.touchCount; i++){
					Touch touch = Input.GetTouch(i); // Get the current touch
					
					// Do the following code for when the touch BEGAN
					if(touch.phase == TouchPhase.Began)
					{
						DoStuff(touch.position);
					} // End if touchphase.began
					
				} // End for each touch
			} // end if there are any touches
		} // end if unity remote
		else // use the normal PC inputs
		{
			if(Input.GetMouseButtonDown(0))
			{
				DoStuff(Input.mousePosition);
			} // End if MouseButtonDown(0)
		}
		#endif
		
		/*
		// If on PC, use the following code
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			DoStuff(Input.mousePosition);
		} // End if MouseButtonDown(0)
		#endif
		*/
	}
	
	// Stuff to do after either touching or using the mouse button
	void DoStuff(Vector3 touchPosition)
	{
		// Bit shift the index of the Clickable layer (9) to only interact with that layer
		int layerMask = 1 << 9;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint (touchPosition), Vector2.zero, 10, layerMask);
		
		// If hit an object on the "Clickable" layer
		if(hit.transform != null)
		{
		//	Debug.Log ("<Clickable> Hit: " + hit.transform.name);
			
				// FIRST CHECK STATE OF GAME, THEN ACCORDINGLY DO IT
				// OR JUST CALL A SINGLE GUI CONTROLLER THAT CONTROLS THE STATE OF THE GAME
			// Activate the InGameGUI
			gameMaster.ClickableGUI(hit.transform.name);
			
			/*\ INSERT STEPS HERE:
				 *  1) Activate the Clickable's code
				 *
				 *
				 *
				 *
				\*/
			
		}
		// If nothing special was clicked on, and can fire and game is NOT pause...
		else if(GameCore.gameState == GameCore.GameState.InGame && !GameCore.pauseState && GameCore.fireReady)
		{
			// Tell the player to fire a missile at the mouse's position
			player.GetComponent<PlayerControl>().FireMissile(touchPosition);
		}
	}
}

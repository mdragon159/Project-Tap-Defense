using UnityEngine;
using System.Collections;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

public class InGameGUI : MonoBehaviour {
	
	// GUI - Missile Icon
	GameObject missileIcon;
	SpriteRenderer missileIconSprite;
	public Sprite[] missileIcons = new Sprite[2];
	
	// In-Game Buttons
	public GameObject pauseButton;
	public GameObject restartButton;

	// Use this for initialization
	void Start () {
		
		// Find and set the missileIcon
		missileIcon = GameObject.Find ("MissileIcon");
		missileIconSprite = missileIcon.GetComponent<SpriteRenderer>();
		if(missileIconSprite == null)
		{
			Debug.LogError("Can't find the missile icon!");
		}
		SetMissileIcon(); // Set up the location, size, etc of the missile icon
		
		// Find the PauseButton GO if it hasn't been attached
		if(pauseButton == null)
		{
			Debug.Log ("<GameGUI> Pause Button wasn't attached");
			pauseButton = GameObject.Find ("PauseButton");
		}
		// Find the RestartButton GO if it hasn't been attached
		if(restartButton == null)
		{
			Debug.Log ("<GameGUI> Restart Button wasn't attached");
			restartButton = GameObject.Find ("RestartButton");
		}
		
		// Set the position of the pauseButton
		SetButton(pauseButton, 0.1f, 0, 0f, 1.2f);
		// Offset the button so the corner is in the top left rather than the center
		ApplyOffset(pauseButton, 0.5f, +0.5f);
		// Similar to the above, but with restart in BOTTOM LEFT
		SetButton(restartButton, 0.1f, 0, 1f, 1.2f);
		ApplyOffset(restartButton, 0.5f, -0.5f);
		
		// Set the Z-coordinates to be on top of everything else
		ChangeZCoord(pauseButton.transform, -1f);
		ChangeZCoord(restartButton.transform, -1f);
	}
	
	// Update is called once per frame
	void Update () {
		// Update the missile icon
		SetMissileIconState ();
	}
	
	// Temporary solution
	void OnGUI()
	{	
		GUI.skin.box.fontSize = Screen.height*7/100;
		GUI.Box(new Rect(Screen.width-Screen.width*9/40,0,
							Screen.width*9/40,Screen.height*4/40), "Score: " + GameCore.curScore);
	}
	
	// Set the missile icon's location
	void SetMissileIcon()
	{	
		Vector2 pos = Vector2.zero;
		// Set location of the missile icon
		pos.x = Camera.main.ScreenToWorldPoint( new Vector3( Screen.width/2, 0, 0)).x;
		pos.y = Camera.main.ScreenToWorldPoint( new Vector3( 0, Screen.height, 0)).y - missileIcon.renderer.bounds.size.y/2;
		missileIcon.transform.position = pos;
	}
	
	// Constrols state of the missile icon
	void SetMissileIconState()
	{
		// If can fire...
		if(GameCore.fireReady)
		{
			// Set the missile icon to the fire ready sprite (spot 0)
			missileIconSprite.sprite = missileIcons[0];
		}
		else
		{
			// Set the missile icon to the can't-fire sprite (spot 1)
			missileIconSprite.sprite = missileIcons[1];
		}
	}
	
	void SetButton(GameObject button, float heightMultiplier, float xPos, float yPos, float accuracyMultiplier)
	{
		// Used as a base, for converting pixels to coordinates for sizes
		Vector3 camLowerCorner = Camera.main.ScreenToWorldPoint( Vector3.zero);
		BoxCollider2D thisCollider = button.GetComponent<BoxCollider2D>();
		
		// Set the size of the button to a certain percent of the scren
		// Holds the size for the pauseButton
		Vector2 size = button.transform.localScale;
		
		// Size = heightMultiplier (percentage of) * screen height
		size.x = Camera.main.ScreenToWorldPoint( new Vector3 (Screen.height*heightMultiplier, 0, 0) ).x;
		size.x -= camLowerCorner.x; // These are in coordinates, so change in coordinates = size
		// "Square" button, so x = y size
		size.y = size.x;
		
		// Apply the size
		button.transform.localScale = size;
		
		// Set the position of the top left of the button to a certain point
		Vector2 pos = Vector2.zero;
		pos.x = Camera.main.ScreenToWorldPoint ( new Vector3 (Screen.width*xPos, 0, 0) ).x;
		pos.y = Camera.main.ScreenToWorldPoint( new Vector3 (0, Screen.height*yPos, 0) ).y;
		
		// Apply the position to the pause button finally
		button.transform.position = pos;
		
		// MAY BE UNNECCESSARY: The distance along the top is cuz the sprite itself is not a perfect square
		// Change the size of the box collider for accuracy reasons
		thisCollider.size *= accuracyMultiplier;
	}
	
	// Applies an offset to the button
	void ApplyOffset(GameObject button, float xMultiplier, float yMultiplier)
	{
		// Get the current posiiton
		Vector2 pos = button.transform.position;
		
		// Offset = size*multiplier
		pos.x += xMultiplier*button.renderer.bounds.size.x;
		pos.y += yMultiplier*button.renderer.bounds.size.y;
		
		// Apply the final position
		button.transform.position = pos;
	}
	
	public void ChangeZCoord(Transform obj, float coord)
	{
		Vector3 pos = obj.position;
		pos.z = coord;
		obj.position = pos;
	}
	
	// Activates buttons based off the name of what was clicked
	public void Activate(string clickName)
	{
		// If name == PauseButton's name, then we have a match!
		if(pauseButton.transform.name == clickName) // If pause button, then pause...
		{
			// Play the general MenuClick SFX
			MusicPlayer.Instance.PlayMenuClick();
		
			// Tell the game to (un)pause
			GameMaster.gmInstance.PauseGame();
		}
		// If restart button, then restart...
		else if(restartButton.transform.name == clickName)
		{
			// Play the general MenuClick SFX
			MusicPlayer.Instance.PlayMenuClick();
			
			// Tell the game to restart
			GameMaster.gmInstance.RestartGame();
		}
	}
}

using UnityEngine;
using System.Collections;

public class GUIColliderSetter : MonoBehaviour {

	public bool roundButton = false; // Indicates if this is a round texture, and if so, use the same width and height
	public bool useOffset;
	// W, X determine width. X, Y determine multiplier of offset (100 = 1, 200 = 2, -100 = -1, etc)
	public Rect guiInfoInPercents; // The position and size of the gui, in percent of the screen (ie 80 = 80% of screen)
	
	// guiWidth and guiHeight still used
	float guiX, guiY, guiWidth, guiHeight;
	float xPos = 0f, yPos = 0f, scaledWidth, scaledHeight;
	
	GameObject thisCollider_go; // Holds the child gameobject of this object that contains the collider
	GameObject thisGUI_go; // Holds the child of this gameObject that holds the guiTexture
	
	// Following may be repetitive and not worth caching, depending on how often they are used... or make the above temp. variables
	BoxCollider2D thisCollider; // Holds this objects Box Collider2D to set it to the correct position
	GUITexture thisGUI; // Uses this to get the size of the texture

	// Use this for initialization
	void Start () {
		// Get the child GameObject that holds the guiTexture
		thisGUI_go = transform.Find ("guiTexture").gameObject;
		if(thisGUI_go == null) Debug.LogError("<GUI> Forgot a GUI Texture...?");
		// Get the GUITexture component
		thisGUI = thisGUI_go.GetComponent<GUITexture>();
		
		// Get the child that holds the BoxCollider2D component
		thisCollider_go = transform.Find ("guiCollider").gameObject;
		if(thisCollider_go == null) Debug.LogError("<GUI> Forgot a GUI Box Collider...?");
		// Get 
		thisCollider = thisCollider_go.GetComponent<BoxCollider2D>();
		
		// convert guiInfoInPercents to guiInfo- in percent form
		guiX = guiInfoInPercents.x/100;
		guiY = guiInfoInPercents.y/100;
		guiWidth = guiInfoInPercents.width/100;
		guiHeight = guiInfoInPercents.height/100;
		
		SetTexture();
		SetCollider();
	}
	
	// If needed, set the texture up
	void SetTexture()
	{
		// Get the values of the position and offset of the guiTexture;
		// Get the width and height of the texture
		scaledWidth = Screen.width*guiWidth;
		scaledHeight = Screen.height*guiHeight;
		// Get the offset in pixels of the texture
//		xPos = Screen.width*guiX;
//		yPos = Screen.height*guiY;
		
		// Apply the above to the guiTexture itself
		if(roundButton) // if this is a round button, make the height and width the same
		{
			if(useOffset)
			{
				xPos = scaledHeight*guiInfoInPercents.x/100f;
				yPos = scaledHeight*guiInfoInPercents.y/100f;
			}
			
			thisGUI.pixelInset = new Rect(xPos, yPos, scaledHeight, scaledHeight);
		}
		else
		{
			if(useOffset)
			{
				xPos = scaledWidth*guiInfoInPercents.x/100f;
				yPos = scaledHeight*guiInfoInPercents.y/100f;
			}
			thisGUI.pixelInset = new Rect(xPos, yPos, scaledWidth, scaledHeight);
		}
	}
	
	// Set the collider up to match the size and location of the texture
	void SetCollider()
	{
		/*|
		 * ~ Set the center of the collider to the center of the gui
		 * ~ Set the size of the collider to the size of the gui (pixels to world coords)
		 *
		 *
		\*/
		
		// Set the size of the collider
		Vector2 size = thisCollider.size; // Holds the current size of the collider, may be redundant
		// Difference between two points = displacement = "size"
		size.x = Camera.main.ScreenToWorldPoint( new Vector3 (thisGUI.pixelInset.width, 0, 0) ).x;
		size.x -= Camera.main.ScreenToWorldPoint( Vector3.zero).x;
		size.y = Camera.main.ScreenToWorldPoint( new Vector3 (thisGUI.pixelInset.height, 0, 0) ).x;
		size.y -= Camera.main.ScreenToWorldPoint( Vector3.zero).y;
		// Apply the size to the collider
		thisCollider.size = size;
		
		// Set the center of the collider to the center of the gui
		Vector2 pos = Vector2.zero;
		Vector2 guiTransOffset = thisGUI_go.transform.position; // Gui texture's offset from transform
		print("guiTransOffset: " + guiTransOffset);
		
		// Convert those "screen" coordinates to world coordinates
		pos.x = Camera.main.ScreenToWorldPoint( new Vector3( Screen.width*guiTransOffset.x, 0, 0) ).x;
		pos.x += size.x/2;
		pos.y = Camera.main.ScreenToWorldPoint( new Vector3( 0, Screen.height*guiTransOffset.y, 0) ).y;
		pos.y -= size.y/2;
		
		/* Learn to apply the offset tho!
		// If used an offset to move the collider
		if(useOffset)
		{
			pos.y += Camera.main.ScreenToWorldPoint( new Vector3( 0, yPos, 0) ).y;
			pos.y -= Camera.main.ScreenToWorldPoint( Vector3.zero).y;
		}
		
		*/
		
		// Apply the position to the collider
		thisCollider_go.transform.position = pos;
	}
}

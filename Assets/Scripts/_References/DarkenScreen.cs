using UnityEngine;
using System.Collections;

public class DarkenScreen : MonoBehaviour {

	private static DarkenScreen m_Instance = null;
//	private bool m_Fading = false;
	private static DarkenScreen Instance
	{
		get
		{
			if (m_Instance == null)
			{
				// Create a new ScreenDarkner just in case
				m_Instance = (new GameObject("ScreenDarkener")).AddComponent<DarkenScreen>();
				// Make sure it's a child of the main camera (for ease of placement and other things)
				m_Instance.transform.parent = Camera.main.transform;
			}
			return m_Instance;
		}
	}
	
/*	public static bool Fading
	{
		get { return Instance.m_Fading; }
	}
*/
	
	private void Awake()
	{
		// Assign the m_instance to this object
		m_Instance = this;
		// Disable the renderer, so it's not blocking anything
		Instance.renderer.enabled = false;
	}
	
	private void Activate(float alpha)
	{
		// Let the world see this screen darkner
		renderer.enabled = true;
		
		// Change the alpha
		Color color = renderer.material.color;
		color.a = alpha;
		renderer.material.color = color;
	}
	
	private void Deactivate()
	{
		// Hide the ScreenDarkner- thus deactivating it
		renderer.enabled = false;
	}
	
	public static void BlockScreen(float alpha)
	{
		Instance.Activate(alpha);
	}
	
	// Set the z-Coordinate AND block the screen
	public static void BlockScreen(float alpha, float zCoord)
	{
		
	}
	
	public static void UnBlockScreen()
	{
		Instance.Deactivate();
	}
} // end DarkenScreen

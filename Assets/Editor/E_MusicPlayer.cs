using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicPlayer))]
public class E_MusicPlayer : Editor
{

	// Flag variable for folds
	bool showDefault = false;	// Show default Inspector?
	bool showExplosions = false; // Show the explosion clips
	bool showGeneral = false; // Show the general SFXs?
	
	public override void OnInspectorGUI()
	{		
		// Get the current script and its values
		MusicPlayer myTarget = (MusicPlayer) target;
		
		// Signal to use GUI.changed, for setting this dirty and saving it
		GUI.changed = false;
		
		myTarget.blockAudio = EditorGUILayout.Toggle("Block Audio?", myTarget.blockAudio);
		
		// Holds the game BGM(s)
		myTarget.mainBGM = EditorGUILayout.ObjectField("Game BGM", myTarget.mainBGM,
		                                           typeof(AudioClip), true) as AudioClip;
		
		// Hide all the general clips unless necessary
		showGeneral = EditorGUILayout.Foldout(showGeneral, "Show General Clips");
		if(showGeneral)
		{
			EditorGUILayout.HelpBox("General SFXs"
			                        ,MessageType.Info);
			
			myTarget.menuClick = EditorGUILayout.ObjectField("Menu Click SFX", myTarget.menuClick,
			                                                     typeof(AudioClip), true) as AudioClip;
			myTarget.missileLaunch = EditorGUILayout.ObjectField("Missile Laucnh SFX", myTarget.missileLaunch,
			                                                     typeof(AudioClip), true) as AudioClip;
			
			EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		}
		
		// Hide all the explosion clips until necessary
		showExplosions = EditorGUILayout.Foldout(showExplosions, "Show Explosion Clips");
		if(showExplosions)
		{
			EditorGUILayout.HelpBox("Explosion SFX. 01 is used for general explosions, like enemy and weapon explosions. "
									+ "02 is used for special explosions, like the player."
			                        ,MessageType.Info);
			myTarget.explosion01 = EditorGUILayout.ObjectField("Explosion01", myTarget.explosion01,
			                                               typeof(AudioClip), true) as AudioClip;
			myTarget.explosion02 = EditorGUILayout.ObjectField("Explosion02", myTarget.explosion02,
			                                                   typeof(AudioClip), true) as AudioClip;
			EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		}
		
		EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		
		// Code for showing the default Inspector
		showDefault = EditorGUILayout.Foldout(showDefault, "Show Default Inspector");
		if(showDefault)
		{
			DrawDefaultInspector();
		}
		
		// If anything was changed, then tell Unity that this prefab was changed so it saves
		if(GUI.changed)
		{
			EditorUtility.SetDirty(myTarget);
		}
	} // End OnInspectorGUI
}

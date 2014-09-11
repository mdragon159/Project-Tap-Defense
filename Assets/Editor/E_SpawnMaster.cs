using UnityEngine;
using UnityEditor;

// Contains necessary core variables and methods for dTT
using com.dTT.Core;

[CustomEditor(typeof(SpawnMaster))]
public class E_SpawnMaster : Editor {

	// Flag variable for folds
	bool showDefault = false;	// Show default Inspector?
	bool showStatistics = false; // Show the SpawnCore statistics?
	bool showDifficultyStats = false; // Show the wave's difficulty settings?
	
	public override void OnInspectorGUI()
	{		
		// Get the current script and its values
		SpawnMaster myTarget = (SpawnMaster) target;
		
		// Signal to use GUI.changed, for setting this dirty and saving it
		GUI.changed = false;
		
		// The necessary Pools
		myTarget.activePool = EditorGUILayout.ObjectField("Active Pool (GO)", myTarget.activePool,
		                                                  typeof(GameObject), true) as GameObject;
		myTarget.spawnPool0 = EditorGUILayout.ObjectField("Spawn Pool0 (Transform)", myTarget.spawnPool0,
		                                                  typeof(SpawnPool), true) as SpawnPool;
		
		EditorGUILayout.LabelField (""); // Just an empty line as a smaller separater
		
		// Displays SpawnCore statistics
		showStatistics = EditorGUILayout.Foldout(showStatistics, "Show Spawn Statistics");
		if(showStatistics)
		{
			EditorGUILayout.HelpBox("Spawn Statistics of the entire game"
			                        ,MessageType.Info);
			
			GUILayout.Box("Total Spawned: " + SpawnCore.totalSpawned);
			GUILayout.Box("Current Wave: " + SpawnCore.currentWave);
			GUILayout.Box("This Wave's Goal: " + SpawnCore.thisWave_goal);
			GUILayout.Box("This Wave's Spawned: " + SpawnCore.thisWave_spawned);
			GUILayout.Box("This Wave's Destroyed: " + SpawnCore.thisWave_defeated);
			
			EditorGUILayout.LabelField("<----->"); // Spacer/Separator
		}
		
		// Displays SpawnCore statistics
		showDifficultyStats = EditorGUILayout.Foldout(showDifficultyStats, "Show Spawn Statistics");
		if(showDifficultyStats)
		{
			EditorGUILayout.HelpBox("Difficulty stats for the current wave (" + SpawnCore.currentWave + ")"
			                        ,MessageType.Info);
			
			GUILayout.Box("Spawn Delay: " + SpawnCore.spawnDelay);
			GUILayout.Box("Max Attack Delay: " + SpawnCore.attackDelay_Max);
			GUILayout.Box("Min Attack Delay: " + SpawnCore.attackDelay_Min);
			GUILayout.Box("Max Size Multiplier: " + SpawnCore.sizeMultiplier_Max);
			GUILayout.Box("Min Size Multiplier: " + SpawnCore.sizeMultiplier_Min);
			
			EditorGUILayout.LabelField("<----->"); // Spacer/Separator
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

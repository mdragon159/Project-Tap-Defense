using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	
	// Used to access this from anywhere
	public static MusicPlayer instance = null;
	public static MusicPlayer Instance {
		get { return instance; }
	}
	public bool blockAudio = false;
	
	// Music BGM
	public AudioClip mainBGM;
	
		// SFX
	// Explosion SFXs
	public AudioClip explosion01;
	public AudioClip explosion02;
	
	// Misc. SFXs
	public AudioClip menuClick;
	public AudioClip missileLaunch;
	
	// Holds the two AudioSources
		// [0] is BGM, [1] is SFX
	AudioSource[] audio;
	
	// Assign the instance to this object
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject); // May be unneccessary
			return;
		} else {
			instance = this;
		}
	
		// No need for DontDestroyOnLoad for this game- simple menus, no need for continued music between Menu and Game
	//	DontDestroyOnLoad(this.gameObject);
	}
	
	void Start()
	{
		// Get the audioSources
		audio = gameObject.GetComponents<AudioSource>();
	//	print("<MusicP> Num of audioSources: " + audioSources.Length);
	
		#if UNITY_EDITOR
		// If set to blocking audio, don't let audio play
		if(blockAudio) return;
		#endif
	
		// Play the assigned BGM, if it exists
		if(mainBGM != null)
		{
			// Load the clip
			audio[0].clip = mainBGM;
			// Play the clip
			audio[0].Play();
		}
	}
	
	// A more common explosion that is frequently heard
	public void PlayExplosion01()
	{
		#if UNITY_EDITOR
		// If set to blocking audio, don't let audio play
		if(blockAudio) return;
		#endif
		
		// Play the explosion sound
		//	audio[1].PlayOneShot(explosion01, 0.8f);
		
		// Randomize the pitch for randomness effect
		audio[1].pitch = Random.Range(0.7f, 1.3f);
		// Change the volume so it doesn't sound too loud
		audio[1].volume = 0.8f;
		
		// Load the clip
		audio[1].clip = explosion01;
		// Play the clip- can't use PlayOneShot as above need to work
		audio[1].Play();
	}
	
	// A less common, more special explosion- like the player's tank death
	public void PlayExplosion02()
	{
		#if UNITY_EDITOR
		// If set to blocking audio, don't let audio play
		if(blockAudio) return;
		#endif
		
		// Less common
		audio[1].pitch = Random.Range(0.9f, 1.1f);
		// Change the volume so it doesn't sound too loud
		audio[1].volume = 1.5f;
				
		// Load the clip
		audio[1].clip = explosion02;
		// Play the clip- can't use PlayOneShot as above need to work
		audio[1].Play();
	}
	
	// A general menu click sound
	public void PlayMenuClick()
	{
		#if UNITY_EDITOR
		// If set to blocking audio, don't let audio play
		if(blockAudio) return;
		#endif
		
		// Reset the pitch
		audio[1].pitch = 1f;
		// Play the audio once, easily and simply
		audio[1].PlayOneShot(menuClick, 0.8f);
	}
	
	// The sound played every time the player attacks
	public void PlayMissileLaunch()
	{
		#if UNITY_EDITOR
		// If set to blocking audio, don't let audio play
		if(blockAudio) return;
		#endif
	
		// Reset the pitch, so it sounds deeper
		audio[1].pitch = 0.8f;
		// Play the audio once, easily and simply
		audio[1].PlayOneShot(missileLaunch, 0.8f);
	}
}

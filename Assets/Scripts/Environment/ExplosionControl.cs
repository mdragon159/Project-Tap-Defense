using UnityEngine;
using System.Collections;

public class ExplosionControl : MonoBehaviour {

	public float selfDestructTime = 0.7f; // How long the animation takes

	// Use this for initialization
	void Start () {
		// Start the self destruct timer for this explosion
		StartCoroutine(DestroySelf(selfDestructTime));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Wait to destroy self
	IEnumerator DestroySelf(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		
		Destroy(this.gameObject);
	}
}

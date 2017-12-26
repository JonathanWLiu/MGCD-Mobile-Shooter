using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour {

    [SerializeField]
    private AudioClip gameover;

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	public void PlayAudioGameover()
    {
        if (source.isPlaying)
        {
            source.Stop();
            source.loop = false;
        }
        source.PlayOneShot(gameover);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField]
    private AudioClip handgunReload;
    [SerializeField]
    private AudioClip handgunShoot;
    [SerializeField]
    private AudioClip handgunEmpty;
    [SerializeField]
    private AudioClip[] hurt;
    [SerializeField]
    private AudioClip die;

    private AudioSource source;

	// Use this for initialization
	void Awake () {
        source = GetComponent<AudioSource>();
	}
	
	public void PlayAudioShoot()
    {
        source.PlayOneShot(handgunShoot);
    }
    public void PlayAudioEmpty()
    {
        source.PlayOneShot(handgunEmpty,0.5f);
    }
    public void PlayAudioReload()
    {
        source.PlayOneShot(handgunReload);
    }
    public void PlayAudioHurt()
    {
        source.PlayOneShot(hurt[Random.Range(0,hurt.Length-1)]);
    }
    public void PlayAudioDie()
    {
        source.PlayOneShot(die);
    }

}

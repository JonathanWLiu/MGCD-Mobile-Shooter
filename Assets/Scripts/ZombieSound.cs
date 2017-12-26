using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : MonoBehaviour {

    [SerializeField]
    private float zombieSoundTimeMin;
    [SerializeField]
    private float zombieSoundTimeMax;

    [SerializeField]
    private AudioClip[] zombieGrowl;
    [SerializeField]
    private AudioClip zombieHit;
    [SerializeField]
    private AudioClip zombieAttack;

    private AudioSource source;
    private Coroutine soundCoroutine;

	// Use this for initialization
	void Awake () {
        source = GetComponent<AudioSource>();
	}
	
	public void PlaySoundHit()
    {
        source.PlayOneShot(zombieHit);
    }
    public void PlaySoundAttack()
    {
        source.PlayOneShot(zombieAttack);
    }
    public void StartZombieSound()
    {
        soundCoroutine = StartCoroutine(PlayZombieSound());
    }
    public void StopZombieSound()
    {
        StopCoroutine(soundCoroutine);
    }
    IEnumerator PlayZombieSound()
    {
        source.PlayOneShot(zombieGrowl[Random.Range(0, zombieGrowl.Length - 1)]);
        yield return new WaitForSeconds(Random.Range(zombieSoundTimeMin, zombieSoundTimeMax));
        soundCoroutine = StartCoroutine(PlayZombieSound());
    }
}

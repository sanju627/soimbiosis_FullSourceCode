using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
	public AudioClip[] BodyFallSFX;
	public AudioClip[] hurtSFX;
    public AudioClip[] footsSFX;

	AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BodyFall()
    {
        audio.PlayOneShot(BodyFallSFX[Random.Range(0, BodyFallSFX.Length)]);
    }

    public void Hurt()
    {
        audio.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
    }

    public void Foots()
    {
        audio.PlayOneShot(footsSFX[Random.Range(0, footsSFX.Length)]);   
    }
}

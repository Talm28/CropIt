using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instence;
    
    public AudioSource musicSource;
    public AudioSource soundSource;

    void Awake()
    {
        if(instence == null)
        {
            instence = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        soundSource = transform.GetChild(1).GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}

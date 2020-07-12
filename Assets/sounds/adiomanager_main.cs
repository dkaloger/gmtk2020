using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class adiomanager_main : MonoBehaviour
{
    AudioClip test;
    public sound[] sounds; // coin pickup
    public sound[] sounds_plant; // coin pickup
    public sound[] sounds_hurt; // coin pickup
    public int soundclips;
    // Start is called before the first frame update
    void Awake()
    {
        if(soundclips == 1)
        {
            foreach (sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
        if (soundclips == 2)
        {
            foreach (sound s in sounds_plant)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
        if (soundclips == 3)
        {
            foreach (sound s in sounds_hurt)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
    }


    // Update is called once per frame
    public void Play()
    {
      sound s =  sounds[Random.Range(0, sounds.Length)];
        s.source.Play();
    }
    public void Play_plant()
    {
        sound s = sounds_plant[Random.Range(0, sounds_plant.Length)];
        s.source.Play();
    }
    public void Play_hurt()
    {
        sound s = sounds_hurt[Random.Range(0, sounds_hurt.Length)];
s.source.Play();
    }
}

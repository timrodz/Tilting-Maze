using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        // Check if there is another instance of the same type and destroy it
        if (Instance != null & Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        // Add the audio sources
        foreach(Sound s in sounds) {

            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }

    }

    public void PlayWithRandomPitch(string name, float min, float max) {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            return;
        }

        s.RandomizePitch(min, max);
        s.source.Play();

    }

    public void Play(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            return;
        }

        s.source.Play();

    }

}
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {

    public string name = "";

    public AudioClip clip = null;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(-3, 3)]
    public float pitch = 1;
	
	public bool loop = false;

    [HideInInspector]
    public AudioSource source = null;

    public void RandomizePitch(float min, float max) {

        if (min < -3) {
            min = -3;
		}
		
		if (max > 3) {
			max = 3;
		}

        source.pitch = Random.Range(min, max);

    }

}
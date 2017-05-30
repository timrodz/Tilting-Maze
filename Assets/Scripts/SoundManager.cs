using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance {
        get; private set;
    }

    [HeaderAttribute ("Audio Sources")]
    public AudioSource sfx;
    public AudioSource music;

    [HeaderAttribute ("Sound effects")]
    public AudioClip moveLeft;
    public AudioClip moveRight;
    public AudioClip slide;
    public AudioClip hit;
    public AudioClip triggerButton;
    public AudioClip pause;

    void Awake () {

        // Check if there is another instance of the same type and destroy it
        if (Instance != null & Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

    }

    public void PlayAudio(AudioClip clip) {

        sfx.clip = clip;
        sfx.Play();

    }

    public void Play (Clip clip) {
        switch (clip) {
            case Clip.moveLeft:
                sfx.PlayOneShot (moveLeft);
                break;
            case Clip.moveRight:
                sfx.PlayOneShot (moveRight);
                break;
            case Clip.slide:
                sfx.PlayOneShot (slide);
                break;
            case Clip.hit:
                sfx.PlayOneShot (hit);
                break;
            case Clip.triggerButton:
                sfx.PlayOneShot (triggerButton);
                break;
            case Clip.pause:
                sfx.PlayOneShot (pause);
                break;

        }

    }

    public void PlayMusic () {
        if (!music.isPlaying) {
            music.loop = true;
            music.Play ();
        }
    }

    public void StopMusic () {
        if (music.isPlaying) {
            music.Stop ();
        }
    }

}
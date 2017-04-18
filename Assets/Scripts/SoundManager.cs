using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	[HeaderAttribute("Audio Sources")]
	public AudioSource sfx;
	public AudioSource music;
	
	[HeaderAttribute("Sound effects")]
	public AudioClip moveLeft;
	public AudioClip moveRight;
	public AudioClip movement;
	public AudioClip hit;
	public AudioClip triggerButton;
	public AudioClip pause;

	public void Play(AudioClip clip) {
		sfx.PlayOneShot(clip);
	}
	
	public void StopMusic() {
		music.Stop();
	}
	
}
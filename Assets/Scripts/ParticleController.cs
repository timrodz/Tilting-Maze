using UnityEngine;

public class ParticleController : MonoBehaviour {
	
	[SerializeField]
	private ParticleSystem topLeft, topRight, bottomLeft, bottomRight;

	public void Play() {
		
		topLeft.Play();
		topRight.Play();
		bottomLeft.Play();
		bottomRight.Play();
		
	}
	
	public void Stop() {
		
		topLeft.Stop();
		topRight.Stop();
		bottomLeft.Stop();
		bottomRight.Stop();
		
	}
	
}
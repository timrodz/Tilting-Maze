using UnityEngine;
using System.Collections.Generic;

public class ParticleController : MonoBehaviour {
	
	[SerializeField]
	private List<ParticleSystem> particleList = new List<ParticleSystem>();

	public void Play() {
		
		foreach (ParticleSystem p in particleList) {
			p.Play();
		}
		
	}
	
	public void Stop() {
		
		foreach (ParticleSystem p in particleList) {
			p.Stop();
		}
		
	}
	
}
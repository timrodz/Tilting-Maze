using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class ParticleFollowTouch : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem m_Particles;
	ParticleSystem p;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		// Lean Touch
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerSet += OnFingerSet;
		LeanTouch.OnFingerUp += OnFingerUp;
		// LeanTouch.OnFingerTap += OnFingerTap;
		// LeanTouch.OnFingerSwipe += OnFingerSwipe;
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable()
	{
		// Lean Touch
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerSet -= OnFingerSet;
		LeanTouch.OnFingerUp -= OnFingerUp;
		// LeanTouch.OnFingerTap -= OnFingerTap;
		// LeanTouch.OnFingerSwipe -= OnFingerSwipe;
	}

	// Use this for initialization
	void Start()
	{
		p = m_Particles;
	}

	public void OnFingerDown(LeanFinger _finger)
	{
		StopAllCoroutines();
		// Lerp particles to center
		StartCoroutine(ParticleLerp(0.25f, false));

	}

	IEnumerator ParticleLerp(float _duration, bool _reset = false)
	{
		Debug.LogFormat("Particle Lerp: {0}", _reset);
		var shapeModule = p.shape;
		shapeModule.sphericalDirectionAmount = _reset ? 0.0f : 1.0f;
		
		for (float t = 0.0f; t <= 1.0f; t += (Time.deltaTime / _duration))
		{
			shapeModule.sphericalDirectionAmount += _reset ? -t : t;
			yield return null;
		}
		
		shapeModule.sphericalDirectionAmount = _reset ? 0.0f : 1.0f;
	}

	public void OnFingerSet(LeanFinger _finger)
	{

	}

	public void OnFingerUp(LeanFinger _finger)
	{
		StopAllCoroutines();
		StartCoroutine(ParticleLerp(0.25f, true));
	}
}
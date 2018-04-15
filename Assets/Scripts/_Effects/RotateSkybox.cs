using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
	[SerializeField]
	private bool rotate = true;

	[SerializeField]
	private float speed = 1.0f;

	// Update is called once per frame
	void Update ()
	{
		if (rotate)
		{
			RenderSettings.skybox.SetFloat ("_Rotation", Time.time * speed);
		}
	}
}
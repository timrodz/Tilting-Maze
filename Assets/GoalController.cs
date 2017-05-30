using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalController : MonoBehaviour {

	public UnityEvent OnCollision;

	public void InvokeOnCollision() {
		OnCollision.Invoke();
	}
	
}
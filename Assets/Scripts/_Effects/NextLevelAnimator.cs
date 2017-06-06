using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NextLevelAnimator : MonoBehaviour {
	
	public TextMeshProUGUI text;
	
	public float duration = 0.5f;
	
	public Ease EaseType;
	
	private const int MAX_VALUE = 2500;
	
	public void ChangeLevelText(string value) {
			
		StartCoroutine(Animate(value));
		
	}
	
	private IEnumerator Animate(string value) {
		
		transform.localScale = Vector3.one;
		transform.DOMoveY(MAX_VALUE, 0);
		
		yield return new WaitForSeconds(2.5f);
		
		// Show the text
		// transform.DOScale(1, duration).SetEase(EaseType);
		transform.DOLocalMove(Vector3.zero, duration).SetEase(EaseType);
		
		yield return new WaitForSeconds(duration * 2);
		
		// Move it downwards
		transform.DOMoveY(-MAX_VALUE, duration).SetEase(EaseType);
		
		yield return new WaitForSeconds(duration * 0.5f);
		
		Debug.Log("new Text");
		
		// Move it upwards and change the value
		transform.DOMoveY(MAX_VALUE, 0);
		text.text = value;
		
		// Move the new text to the center
		transform.DOLocalMove(Vector3.zero, duration).SetEase(EaseType);
		
		yield return new WaitForSeconds(duration * 2f);
		
		// Move it downwards
		transform.DOMoveY(-MAX_VALUE, duration).SetEase(EaseType);
		
	}
	
}
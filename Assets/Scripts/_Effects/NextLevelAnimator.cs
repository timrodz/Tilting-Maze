using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NextLevelAnimator : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float duration = 0.5f;

    public Ease EaseType;

    private const int MAX_VALUE = 2500;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

        transform.DOScale(Vector3.zero, 0);

    }

    public void ChangeLevelText(string value)
    {

        StartCoroutine(Animate(value));

    }

    private IEnumerator Animate(string value)
    {

        transform.localScale = Vector3.one;
        transform.DOMoveY(MAX_VALUE, 0);

        yield return new WaitForSeconds(2.75f);

        // Show the text
        // transform.DOScale(1, duration).SetEase(EaseType);
        transform.DOLocalMove(Vector3.zero, duration).SetEase(EaseType);

        yield return new WaitForSeconds(duration * 2);

        // Move it downwards
        transform.DOMoveY(-MAX_VALUE, duration).SetEase(EaseType);

        yield return new WaitForSeconds(duration * 0.5f);

        Debug.Log("Animating new level text: #" + value.ToString());

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
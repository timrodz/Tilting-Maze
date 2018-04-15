using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NextLevelAnimator : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_Transparency;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private RectTransform m_RectTransform;

    public float duration = 0.5f;

    public Ease EaseType;

    [SerializeField] private int MAX_VERTICAL_OFFSET = 2500;

    void Awake()
    {
        Game_Events.Instance.OnLevelComplete += OnLevelComplete;

        if (null == m_Transparency)
        {
            m_Transparency = GetComponent<CanvasGroup>();
        }

        if (null == m_Text)
        {
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (null == m_RectTransform)
        {
            m_RectTransform = GetComponent<RectTransform>();
        }
    }

    void Start()
    {
        m_Transparency.alpha = 1;
        m_RectTransform.localScale = Vector3.zero;
        m_Text.text = (GameManager.Instance.GetLevelID() + 1).ToString();
    }

    public void ChangeLevelText(string value)
    {
        StartCoroutine(Animate(value));
    }

    private IEnumerator Animate(string value)
    {        
        Debug.Log("Animating new level text: #" + value.ToString());
        
        m_RectTransform.localScale = Vector3.one;
        m_RectTransform.DOAnchorPosY(MAX_VERTICAL_OFFSET, 0);

        yield return new WaitForSeconds(2.75f);

        // Show the text
        m_RectTransform.DOAnchorPosY(0, duration).SetEase(EaseType);

        yield return new WaitForSeconds(duration * 2);

        // Move it downwards
        m_RectTransform.DOAnchorPosY(-MAX_VERTICAL_OFFSET, duration).SetEase(EaseType).OnComplete(() =>
        {
            // Move it upwards and change the value
            m_RectTransform.anchoredPosition = new Vector2(0, MAX_VERTICAL_OFFSET);
        });

        yield return new WaitForSeconds(duration * 2);

        Debug.Log("Animating new level text: #" + value.ToString());
        
        Debug.Log("Position: " + m_RectTransform.anchoredPosition.y);
        m_Text.text = value;

        // Move the new text to the center
        m_RectTransform.DOAnchorPosY(0, duration).SetEase(EaseType);

        yield return new WaitForSeconds(duration * 2f);

        // Move it downwards
        m_RectTransform.DOAnchorPosY(-MAX_VERTICAL_OFFSET, duration).SetEase(EaseType);
    }

    public void OnLevelComplete(int _levelID)
    {
        string result = (_levelID == -1) ? ("<size=40>Thank you for playing squared cycles. more levels are to come!\n\n<size=24>Follow @timrodz and @squaredcycles to learn more.") : ((_levelID + 1).ToString());

        StartCoroutine(Animate(result));
    }

}
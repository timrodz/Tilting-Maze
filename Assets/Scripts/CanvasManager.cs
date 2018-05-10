using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [Header ("Total moves")]
    [SerializeField] private GameObject m_TotalMovesPanel;
    [SerializeField] public CanvasGroup m_TotalMovesPanelTransparency;
    [SerializeField] private TextMeshProUGUI m_TotalMovesText;
    [SerializeField] private Vector3 m_TotalMovesPosition;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start ()
    {
        if (!m_TotalMovesPanelTransparency)
        {
            m_TotalMovesPanelTransparency = m_TotalMovesPanel.GetComponent<CanvasGroup> ();
        }

        m_TotalMovesPosition = m_TotalMovesText.rectTransform.localPosition;

        Utils.Fade (m_TotalMovesPanelTransparency, false, 0);
    }

    public void ResetTotalMovesPanelPosition ()
    {
        Utils.Fade (m_TotalMovesPanelTransparency, false, 0);

        m_TotalMovesText.rectTransform.DOLocalMove (m_TotalMovesPosition, 0);

    }

    public void SetTotalMovesText (string _text)
    {
        m_TotalMovesText.text = _text;
    }

}
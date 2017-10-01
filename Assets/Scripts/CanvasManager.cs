﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    [Header("Total moves")]
    [SerializeField] private GameObject TotalMovesPanel;
    [SerializeField] public CanvasGroup TotalMovesPanelTransparency;
    [SerializeField] private TextMeshProUGUI totalMovesText;
    [SerializeField] private Vector3 totalMovesPosition;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Check if there is another instance of the same type and destroy it
        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (!TotalMovesPanelTransparency)
        {
            TotalMovesPanelTransparency = TotalMovesPanel.GetComponent<CanvasGroup>();
        }

        totalMovesPosition = totalMovesText.rectTransform.localPosition;

        Utils.Fade(TotalMovesPanelTransparency, false, 0);
    }

    public static void ResetTotalMovesPanelPosition()
    {
        if (null == CanvasManager.Instance)
        {
            return;
        }
        
        Utils.Fade(CanvasManager.Instance.TotalMovesPanelTransparency, false, 0);
        
        CanvasManager.Instance.totalMovesText.rectTransform.DOLocalMove(CanvasManager.Instance.totalMovesPosition, 0);

    }
    
    public static void SetTotalMovesText(string _text)
    {
        if (null == CanvasManager.Instance)
        {
            return;
        }
        
        CanvasManager.Instance.totalMovesText.text = _text;
    }

}
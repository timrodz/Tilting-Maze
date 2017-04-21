using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public List<UIElement> UIElementList = new List<UIElement> ();

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {

        foreach(UIElement e in UIElementList) {

            if (XCI.GetButtonDown(e.button) || Input.GetKeyDown(e.key)) {

                CanvasGroup cg = e.gameObject.GetComponent<CanvasGroup> ();
				
				if (cg) {
					cg.DOFade(0, 1);
					cg.blocksRaycasts = false;
				} else {
					e.gameObject.GetComponent<Image>().DOFade(0, 1);
				}
				
				UIElementList.Remove(e);

            }

        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class KeyListener : MonoBehaviour {

    public List<KeyCode> keyList = new List<KeyCode>();
    public List<XboxButton> buttonList = new List<XboxButton>();

    // Update is called once per frame
    void Update() {

        foreach(KeyCode k in keyList) {
            if (Input.GetKeyDown(k)) {
				
			}
        }

    }

}
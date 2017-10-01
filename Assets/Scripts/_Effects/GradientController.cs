using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientController : MonoBehaviour {

    public Material skyboxMaterial;
    public Color topColor;
    public Color botColor;
    [RangeAttribute (0, 0.05f)]
    public float colorChangeDelay = 0;

    private HSBColor topColorHSB;
    private HSBColor botColorHSB;

    // Use this for initialization
    void Start () {

        topColor = skyboxMaterial.GetColor ("_Color2");
        botColor = skyboxMaterial.GetColor ("_Color1");

        topColorHSB = new HSBColor (topColor);
        botColorHSB = new HSBColor (botColor);

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        if (colorChangeDelay == 0) {
            return;
        }
        ChangeColors ();
    }

    public void ChangeColors () {

        topColor = HSBColor.ToColor (new HSBColor (Mathf.PingPong (Time.time * colorChangeDelay, 1), topColorHSB.s, topColorHSB.b));
        botColor = HSBColor.ToColor (new HSBColor (Mathf.PingPong (Time.time * colorChangeDelay, 1), botColorHSB.s, botColorHSB.b));

        skyboxMaterial.SetColor ("_Color2", topColor);
        skyboxMaterial.SetColor ("_Color1", botColor);

    }

}
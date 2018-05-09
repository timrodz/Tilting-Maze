using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public Material material;
    public Color color;
    [RangeAttribute (0.01f, 0.05f)]
    public float colorChangeDelay = 0;

    public string colorString = "";

    private HSBColor colorHSB;

    // Use this for initialization
    void Start ()
    {
        if (!material || colorString == "")
        {
            Destroy (this);
        }

        color = material.GetColor ("_" + colorString);

        colorHSB = new HSBColor (color);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update ()
    {
        ChangeColors ();
    }

    public void ChangeColors ()
    {
        color = HSBColor.ToColor (new HSBColor (Mathf.PingPong (Time.time * colorChangeDelay, 1), colorHSB.s, colorHSB.b));

        material.SetColor ("_" + colorString, color);
    }

}
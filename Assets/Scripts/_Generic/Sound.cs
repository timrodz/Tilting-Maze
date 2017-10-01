using UnityEngine;

[System.Serializable]
public enum ClipType
{
    None,
    Move_L,
    Move_R,
    Trigger_Button,
    Collision
}

[System.Serializable]
public class Sound
{
    public ClipType clipType = ClipType.None;
    public AudioClip clip = null;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(-3, 3)]
    public float pitch = 1;
    public bool loop;
}
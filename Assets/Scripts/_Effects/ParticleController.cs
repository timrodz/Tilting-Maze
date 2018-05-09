using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public Transform particleContainer;

    private List<ParticleSystem> particleList = new List<ParticleSystem> ();

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start ()
    {
        if (!particleContainer)
        {
            Destroy (gameObject);
        }

        for (int i = 0; i < particleContainer.childCount; i++)
        {
            ParticleSystem p = particleContainer.GetChild (i).GetComponent<ParticleSystem> ();

            if (p != null)
            {
                particleList.Add (p);
            }
        }
    }

    public void Play ()
    {
        foreach (ParticleSystem p in particleList)
        {
            p.Play ();
        }
    }

    public void Stop ()
    {
        foreach (ParticleSystem p in particleList)
        {
            p.Stop ();
        }
    }

}
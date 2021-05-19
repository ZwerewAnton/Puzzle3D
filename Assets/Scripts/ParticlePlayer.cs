using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] clips;

    public void Play()
    {
        foreach(ParticleSystem clip in clips)
        {
            clip.Play();
        }
    }
}

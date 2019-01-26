using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : Singleton<SnowController>
{
    [SerializeField] private ParticleSystem snowStormParticles;

    public void PlayStormParticles()
    {
        snowStormParticles.Play();
    }
}

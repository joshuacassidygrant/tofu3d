using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVFXParticle : AbstractAVFXComponent
{
    private ParticleSystem _particleSystem;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        
    }

    public override void Play()
    {
        _particleSystem.Play();
    }

    public override void Stop()
    {
        _particleSystem.Stop();
    }


}

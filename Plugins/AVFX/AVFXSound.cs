using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVFXSound : AbstractAVFXComponent
{
    private AudioSource _audioSource;

    void Awake() {
        _audioSource = GetComponent<AudioSource>();

    }

    public override void Play() {
        _audioSource.Play();
    }

    public override void Stop() {
        _audioSource.Stop();
    }
}

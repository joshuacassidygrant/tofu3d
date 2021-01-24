using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Scripts.AVFX;
using UnityEngine;

public class AVFXInstance: MonoBehaviour
{
    public float TotalLifetime;
    public float Lifetime;
    public int FireIndex;

    public float _collideDist;

    private AVFXManager _manager;

    public List<AVFXInstanceEntry> Components;

    public void Initialize(float lifetime)
    {
        Lifetime = lifetime;

    }

    void Start()
    {
        Components.Sort((x, y) => x.Delay.CompareTo(y.Delay));
    }

    void Update()
    {
        Lifetime += Time.deltaTime;

        while (FireIndex < Components.Count && Components[FireIndex].Delay <= Lifetime)
        {
            Components[FireIndex].Component.Play();
            FireIndex++;
        }

        if (Lifetime >= TotalLifetime && TotalLifetime != 0)
        {
            EndEffect();
        }

    }

    public void BindManager(AVFXManager mgr)
    {
        _manager = mgr;
    }

    public void SetProjectileTarget(Vector3 target)
    {
        transform.LookAt(target);
    }

    public void EndEffect()
    {
        Destroy(gameObject);

    }

    [System.Serializable]
    public struct AVFXInstanceEntry
    {
        public float Delay;
        public AbstractAVFXComponent Component;
    }



}
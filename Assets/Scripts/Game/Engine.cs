using System;
using NaughtyAttributes;
using UnityEngine;

public class Engine : TrainWagonBase
{
    public int Coal;
    public float Sand;
    public const float MaxSand = 6f;

    [SerializeField] private ParticleSystem p_Sys;

    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.ForceOverLifetimeModule force;

    private void Update()
    {
        emission = p_Sys.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(DriveHandler.Instance.Speed/2);
        force = p_Sys.forceOverLifetime;
        force.x = new ParticleSystem.MinMaxCurve(-DriveHandler.Instance.Speed);
        p_Sys.startRotation = Mathf.Deg2Rad * (90f * UnityEngine.Random.Range(0, 4));
    }

}
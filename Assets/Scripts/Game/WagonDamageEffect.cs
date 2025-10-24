using System;
using UnityEngine;

public class WagonDamageEffect : MonoBehaviour
{
    private TrainWagonBase associatedWagon;
    private Train associatedTrain;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AnimationCurve trainDamageToParticleAmountCurve;
    private float size = 1f;
    private void OnEnable()
    {
        associatedWagon = GetComponentInParent<TrainWagonBase>();
        associatedTrain = GetComponentInParent<Train>();

        if (associatedTrain)
        {
            associatedTrain.TrainDamageEvent.AddListener(OnTrainDamage);
        }

        if (associatedWagon)
        {
            ModifyDimensions(associatedWagon.Config);
        }
    }
    private void OnDisable()
    {        
        if (associatedTrain)
        {
            associatedTrain.TrainDamageEvent.RemoveListener(OnTrainDamage);
        }
    }
    private void ModifyDimensions(WagonConfigBase wagon)
    {
        size = wagon.Length;
        var shape = particle.shape;
        shape.scale = new Vector3(size / 2f, 1f, 1f);
        shape.position = new Vector3(-size / 2f, 0f, 0f);
    }
    private void OnTrainDamage(float dmg)
    {
        var emit = particle.emission;
        emit.rateOverTime = size * trainDamageToParticleAmountCurve.Evaluate(dmg);
        particle.Play();
    }
}

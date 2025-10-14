using NaughtyAttributes;
using UnityEngine;

public class EngineWagonConfig : WagonConfigBase
{
    [SerializeField, BoxGroup("EngineConfig")] public float CoalBurnRate = .8f;
    [SerializeField, BoxGroup("EngineConfig")] public float MaxAccelleration = 4f;
    [SerializeField, BoxGroup("EngineConfig")] public float BreakPower = 20f;
    [SerializeField, BoxGroup("EngineConfig")] public float SandConsumption = 1f;
}
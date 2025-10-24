using NaughtyAttributes;
using UnityEngine;

public class EngineWagonConfig : WagonConfigBase
{
    [SerializeField, BoxGroup("EngineConfig")] public float MaxSpeed = 45f;
    [SerializeField, BoxGroup("EngineConfig")] public int MaxCoalStorage = 10;
    [SerializeField, BoxGroup("EngineConfig")] public float CoalBurnRate = .8f;
    [SerializeField, BoxGroup("EngineConfig")] public float MaxAccelleration = 4f;
    [SerializeField, BoxGroup("EngineConfig")] public float BreakPower = 20f;
    [SerializeField, BoxGroup("EngineConfig")] public int MaxSandStorage = 10;
    [SerializeField, BoxGroup("EngineConfig")] public float SandConsumption = 1f;
}
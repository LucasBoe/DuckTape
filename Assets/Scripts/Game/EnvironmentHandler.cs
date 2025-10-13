using System;
using System.Collections;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(0,100)] private float currentSpeed => DriveHandler.Instance.Speed;
    [SerializeField, ReadOnly] EnvironmentAssetSpawner[] spawners;

    public const float ASSET_AREA_RADIUS = 20f;

    private void OnValidate()
    {
        spawners = GetComponentsInChildren<EnvironmentAssetSpawner>();
    }
    private void Update()
    {
        if (currentSpeed == 0f)
            return;
        
        float translation = currentSpeed * Time.deltaTime;

        foreach (var spawner in spawners)
            spawner.Refresh(translation);
    }
    public void BeginDrive()
    {
        foreach (var spawner in spawners)
        {
            spawner.DoSpawn = spawner.Condition == SpawnCondition.Allways || spawner.Condition == SpawnCondition.OutsideOfStation;
        }
    }    
    public void EndDrive()
    {
        foreach (var spawner in spawners)
        {
            spawner.DoSpawn = spawner.Condition == SpawnCondition.Allways || spawner.Condition == SpawnCondition.InsideStation;
        }
    }
}
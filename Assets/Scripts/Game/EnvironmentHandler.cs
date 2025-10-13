using System;
using System.Collections;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(0,100)] private float currentSpeed;
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
    public IEnumerator BeginDrive()
    {
        foreach (var spawner in spawners)
        {
            spawner.DoSpawn = spawner.Condition == SpawnCondition.Allways || spawner.Condition == SpawnCondition.OutsideOfStation;
        }
        return LerpSpeedRoutine(25f);     
    }    
    public IEnumerator EndDrive()
    {
        foreach (var spawner in spawners)
        {
            spawner.DoSpawn = spawner.Condition == SpawnCondition.Allways || spawner.Condition == SpawnCondition.InsideStation;
        }
        return LerpSpeedRoutine( 0f);
    }
    private IEnumerator LerpSpeedRoutine(float to)
    {
        while (Mathf.Abs(currentSpeed - to) > 0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, to, Time.deltaTime);
            currentSpeed = Mathf.MoveTowards(currentSpeed, to, Time.deltaTime);
            yield return null;
        }
        
        currentSpeed = to;
    }

}
using System;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(0,100)] private float currentSpeed;
    [SerializeField, ReadOnly] EnvironmentAssetSpawner[] spawners;

    public const float ASSET_AREA_RADIUS = 10f;

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
}
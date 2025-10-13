using System;
using NaughtyAttributes;
using SS;
using UnityEngine;
using Event = SS.Event;

[SingletonSettings(_lifetime: SingletonLifetime.Scene, _canBeGenerated: true, _eager: true)]
public class DriveHandler : SingletonBehaviour<DriveHandler>
{
    [SerializeField, ReadOnly] private Engine currentEngine;
    [SerializeField, ReadOnly] private float currentSpeed;
    [SerializeField, ReadOnly] private float currentAcceleration = 0f;
    [SerializeField, ReadOnly] private float currentSlope;
    
    [SerializeField, ReadOnly] private Section currentSection;
    [SerializeField, ReadOnly] private float currentSectionProgression;
    EngineWagonConfig engineConfig => currentEngine.Config as EngineWagonConfig;
    public Event OnCurrentSectionEndReached = new Event();
    public float Progression => currentSectionProgression;
    public float Speed => currentSpeed;

    public void ModifyEngine(Engine engine)
    {
        currentEngine = engine;
    }    
    public void ModifySection(Section section)
    {
        currentSection = section;
    }
    private void Update()
    {
        if (currentEngine == null)
            return;
        
        currentAcceleration = Mathf.Lerp(currentAcceleration, -4f, Time.deltaTime * engineConfig.CoalBurnRate);
        
        currentSpeed += currentAcceleration * Time.deltaTime + currentSlope * Time.deltaTime;
        
        if (currentSpeed < 0)
            currentSpeed = 0;
        
        if (currentSection == null)
            return;
        
        currentSectionProgression += currentSpeed / currentSection.Length * Time.deltaTime;
        currentSlope = currentSection.SlopeOverSection.Evaluate(currentSectionProgression);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, currentSlope);
        
        if (currentSectionProgression >= 1f)
        {
            OnCurrentSectionEndReached?.Invoke();
            currentSectionProgression = 0f;
            currentSection = null;
        }
    }

    [Button]
    public void Shovel()
    {
        currentAcceleration = Mathf.Lerp(currentAcceleration, engineConfig.MaxAccelleration, .5f);
    }
}

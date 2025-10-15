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
    [SerializeField, ReadOnly] private bool doBreak;
    
    [SerializeField, ReadOnly] private Section currentSection;
    [SerializeField, ReadOnly] private float currentSectionProgression;
    EngineWagonConfig engineConfig => currentEngine.Config as EngineWagonConfig;
    public Event OnCurrentSectionEndReached = new Event();
    public float Acceleration => currentAcceleration;
    public float Progression => currentSectionProgression;
    public float Speed => currentSpeed;
    public float DistanceLeft => currentSection == null ? 0f : (1f - currentSectionProgression) * currentSection.Length;
    public bool DoBreak => doBreak;
    public Engine Engine => currentEngine;

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

        float highAccelerationKeeper = (1f + .5f * engineConfig.MaxAccelleration / currentAcceleration);
        
        currentAcceleration = Mathf.Max(Mathf.Lerp(currentAcceleration,  -.1f, Time.deltaTime * engineConfig.CoalBurnRate * highAccelerationKeeper));
        currentSpeed += currentAcceleration * Time.deltaTime + currentSlope * Time.deltaTime;

        //drag
        currentSpeed *= (1 - Time.deltaTime / 4);
        
        //break
        if (doBreak)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, -1f, Time.deltaTime * engineConfig.BreakPower);
            currentEngine.Sand -= Time.deltaTime * engineConfig.SandConsumption;
        }
        
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
        if (currentEngine.Coal <= 0)
            return;
        
        currentEngine.Coal -= 1;
        currentAcceleration = Mathf.Lerp(currentAcceleration, engineConfig.MaxAccelleration, .5f);
    }

    public void Break()
    {
        doBreak = true;
    }

    public void Unbreak()
    {
        doBreak = false;
    }
}

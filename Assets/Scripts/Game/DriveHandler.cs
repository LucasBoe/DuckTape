using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using SS;
using Unity.Cinemachine;
using UnityEngine;
using Event = SS.Event;

[SingletonSettings(_lifetime: SingletonLifetime.Scene, _canBeGenerated: true, _eager: true)]
public class DriveHandler : SingletonBehaviour<DriveHandler>
{
    [SerializeField, ReadOnly] private Engine currentEngine;
    [SerializeField, ReadOnly] private Train currentTrain;
    [SerializeField, ReadOnly] private float currentSpeed;
    [SerializeField, ReadOnly] private int totalWeight;
    [SerializeField, ReadOnly] private float currentAcceleration = 0f;
    [SerializeField, ReadOnly] private float currentSlope;
    [SerializeField, ReadOnly] private bool doBreak;

    [SerializeField, ReadOnly] private Section currentSection;
    [SerializeField, ReadOnly] private float currentSectionProgression;
    
    [SerializeField, ReadOnly] private bool isInStation = false;
    [SerializeField, ReadOnly] private float currentMetersUntilStop = 100f;
    [SerializeField, ReadOnly] private float maxMetersUntilStop = 100f;
    [SerializeField, ReadOnly] private float speedWhenEnteringStation = 0;
    [SerializeField, ReadOnly] private float accelerationWhenEnteringStation = 0;

    EngineWagonConfig engineConfig => currentEngine.Config as EngineWagonConfig;
    public Event OnCurrentSectionEndReached = new Event();
    public float Acceleration => currentAcceleration;
    public float Progression => currentSectionProgression;
    public float Speed => currentSpeed;
    public int TotalWeight => totalWeight;
    public float DistanceLeft => currentSection == null ? 0f : (1f - currentSectionProgression) * currentSection.Length;
    public bool DoBreak => doBreak;
    public Engine Engine => currentEngine;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(CheckSpeedShakeRoutine());
    }
    private IEnumerator CheckSpeedShakeRoutine()
    {
        float shakeDuration = .2f;
        
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(shakeDuration);
            if (Speed >= 0)
            {
                var shakeStrengthAtCurrentSpeed = GlobalBalancing.Value.ShakeOverTrainSpeedCurve.Evaluate(Speed);
                if (shakeStrengthAtCurrentSpeed >= 0f)
                {
                    currentTrain?.TryShakeWagonsFor(shakeStrengthAtCurrentSpeed, shakeStrengthAtCurrentSpeed);
                }
            } 
        }
    }

    private void OnEnable()
    {
        LoopEventHandler.Instance.OnStationExitEvent.AddListener(OnStationExit);
    }

    private void OnDisable()
    {
        LoopEventHandler.Instance.OnStationExitEvent.RemoveListener(OnStationExit);
    }

    private void OnStationExit()
    {
        totalWeight = currentTrain.CalculateTotalWeight();
    }

    public void AssignTrainInstance(Train train)
    {
        currentTrain = train;
    }

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
        if (isInStation)
        {
            float l = 1f - currentMetersUntilStop / maxMetersUntilStop;
            currentAcceleration = Mathf.Lerp(accelerationWhenEnteringStation, 0f, l);
            currentSpeed = Mathf.Lerp(speedWhenEnteringStation, .1f, l);
            currentMetersUntilStop -= currentSpeed * Time.deltaTime;
        }
        else
        {
            if (currentEngine == null)
                return;

            float highAccelerationKeeper = (1f + .5f * engineConfig.MaxAccelleration / currentAcceleration);
            
            currentAcceleration = Mathf.Max(Mathf.Lerp(currentAcceleration,  -.1f, Time.deltaTime * engineConfig.CoalBurnRate * highAccelerationKeeper));
            var accelerationTroughMass = currentAcceleration / (totalWeight / 1000);
            currentSpeed += accelerationTroughMass * Time.deltaTime + currentSlope * Time.deltaTime;

            //drag
            currentSpeed *= (1 - Time.deltaTime / 4);
            
            //break
            if (doBreak)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, -1f, Time.deltaTime * (engineConfig.BreakPower / (totalWeight / 1000)));
                currentEngine.Sand -= Time.deltaTime * engineConfig.SandConsumption;
            }
            
            if (currentSpeed < 0)
                currentSpeed = 0;
            
            if (currentSection == null)
                return;
            
            currentSectionProgression += currentSpeed / currentSection.Length * Time.deltaTime;
            currentSlope = currentSection.SlopeOverSection.Evaluate(currentSectionProgression);
            currentTrain.Camera.transform.rotation = Quaternion.Euler(0, 0, currentSlope);
            
            if (currentSectionProgression >= 1f)
            {
                OnCurrentSectionEndReached?.Invoke();
                currentSectionProgression = 0f;
                currentSection = null;
            }
        }
    }
    public IEnumerator AnimateToStillIn(float meters)
    {
        isInStation = true;
        currentMetersUntilStop = meters;
        maxMetersUntilStop = meters;
        speedWhenEnteringStation = currentSpeed;
        accelerationWhenEnteringStation = currentAcceleration;

        while (currentSpeed > .1f)
            yield return null;
        
        isInStation = false;
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
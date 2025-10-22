using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PassengerWagon : TrainWagonBase
{
    [SerializeField] private Transform passengerContainer;
    [SerializeField, ReadOnly] private List<SpriteRenderer> passengerVisualisations = new();
    [SerializeField, ReadOnly] private List<Passenger> passengersEntered = new();
    public int MaxPassengerCount => passengerVisualisations.Count;
    public int CurrentPassengerCount => passengersEntered.Count;
    
    private void OnValidate()
    {
        for (int i = 0; i < passengerContainer.childCount; i++)
        {
            var visualization = passengerContainer.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();
            visualization.enabled = false;
            passengerVisualisations.Add(visualization);
        }
    }
    private void OnEnable()
    {
        PassengerHandler.Instance.RegisterAsActiveWagon(this);
    }
    private void OnDisable()
    {
        if (PassengerHandler.InstanceExists)
            PassengerHandler.Instance.UnregisterAsActiveWagon(this);
    }
    public void Enter(Passenger passenger)
    {
        passenger.transform.SetParent(passengerVisualisations[CurrentPassengerCount].transform.parent);
        passenger.transform.localPosition = new Vector2(-0.1275f, -0.1275f);
        passengersEntered.Add(passenger);
        
        RefreshVisuals();
    }
    public void Exit(Passenger passenger)
    {
        passengersEntered.Remove(passenger);
        passenger.ResetParent();
        var localPos = passenger.transform.localPosition;
        passenger.transform.DOLocalMove(localPos - new Vector3(-100f, 0f), 10f);
        
        RefreshVisuals();
    }
    private void RefreshVisuals()
    {
        for (int i = 0; i < passengerVisualisations.Count; i++)
        {
            passengerVisualisations[i].enabled = i < CurrentPassengerCount;
        }
    }
}
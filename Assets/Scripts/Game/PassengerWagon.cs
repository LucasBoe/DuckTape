using System.Collections.Generic;
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
        PassengerHandler.Instance.UnregisterAsActiveWagon(this);
    }
    public void Enter(Passenger passenger)
    {
        passenger.transform.SetParent(passengerVisualisations[CurrentPassengerCount].transform.parent);
        passenger.transform.localPosition = new Vector2(-0.1275f, -0.1275f);
        passengersEntered.Add(passenger);
        
        //refresh visuals
        for (int i = 0; i < passengerVisualisations.Count; i++)
        {
            passengerVisualisations[i].enabled = i < CurrentPassengerCount;
        }
    }
}
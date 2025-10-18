using System.Collections.Generic;
using SS;
using Unity.Collections;
using UnityEngine;

[SingletonSettings(SingletonLifetime.Scene, true)]
public class PassengerHandler : SingletonBehaviour<PassengerHandler>
{
    [SerializeField, ReadOnly] private int currentPassengerCount = 0;
    [SerializeField, ReadOnly] private int maxPassengerCount;
    private Dictionary<PassengerWagon, List<Passenger>> passengers = new();
    public void RegisterAsActiveWagon(PassengerWagon passengerWagon)
    {
        passengers.Add(passengerWagon, new List<Passenger>());
        maxPassengerCount += passengerWagon.MaxPassengerCount;
    }
    public void UnregisterAsActiveWagon(PassengerWagon passengerWagon)
    {
        //kick all passengers?
        foreach (var passenger in passengers[passengerWagon])
        {
            currentPassengerCount--;
        }
        passengers.Remove(passengerWagon);
        maxPassengerCount -= passengerWagon.MaxPassengerCount;
    }
    public bool TryEnterTrain(Passenger passenger)
    {
        if (currentPassengerCount >= maxPassengerCount)
            return false;

        foreach (var pair in passengers)
        {
            if (pair.Value.Count < pair.Key.MaxPassengerCount)
            {
                pair.Value.Add(passenger);
                passenger.AssignWagon(pair.Key);
                currentPassengerCount++;
                return true;
            }
        }
        
        return false;
    }
}
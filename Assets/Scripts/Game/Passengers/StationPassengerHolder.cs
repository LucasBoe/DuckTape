using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

public class StationPassengerHolder : MonoBehaviour
{
    [FormerlySerializedAs("dummy")] [SerializeField] private Passenger prefab;
    List<Passenger> passengers = new();
    private void Awake()
    {
        prefab.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        LoopEventHandler.Instance.OnStationEnterEvent.AddListener(OnStationEnter);
        LoopEventHandler.Instance.OnStationExitEvent.AddListener(OnStationExit);
    }
    private void OnDisable()
    {
        LoopEventHandler.Instance.OnStationEnterEvent.RemoveListener(OnStationEnter);
        LoopEventHandler.Instance.OnStationExitEvent.RemoveListener(OnStationExit);
    }
    private void OnStationEnter()
    {
        var instance = Instantiate(prefab, transform);
        instance.gameObject.SetActive(true);
        instance.transform.localPosition = Vector3.zero;
        passengers.Add(instance);
    }
    private void OnStationExit()
    {
        foreach (var passenger in passengers)
        {
            if (passenger.State == PassengerState.Station)
                Destroy(passenger.gameObject);
        }
        passengers.Clear();
    }
}

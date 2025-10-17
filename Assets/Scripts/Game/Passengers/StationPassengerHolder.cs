using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

public class StationPassengerHolder : MonoBehaviour
{
    [FormerlySerializedAs("dummy")] [SerializeField] private Passenger prefab;
    List<Passenger> passengers = new();
    public int AmountToSpawn;
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
        SpawnPassengers();
    }
    [Button]
    private void SpawnPassengers()
    {
        for (int i = 0; i < AmountToSpawn; i++)
        {
            var instance = Instantiate(prefab, transform);
            instance.gameObject.SetActive(true);
            instance.transform.localPosition = new Vector3(i * .4f, 0f, 0f);
            instance.Init();
            passengers.Add(instance);
        }
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

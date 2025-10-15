using System;
using System.Collections.Generic;
using NaughtyAttributes;
using SS;
using UnityEngine;
using Random = UnityEngine.Random;

public class StationCargoHolder : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private CargoConfigContainer cargos;
    [SerializeField, BoxGroup("References")] private CargoSlot slotDummy;
    [SerializeField, BoxGroup("Balancing")] private Vector2Int minMaxCargoCount;
    [SerializeField, BoxGroup("Balancing")] private float cargoSlotxOffset;
    
    private List<CargoSlot> createdSlots = new();

    private void Awake()
    {
        slotDummy.gameObject.SetActive(false);
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
        //create new cargo
        int maxCargo = Random.Range(minMaxCargoCount.x, minMaxCargoCount.y);
        for (int i = 0; i < maxCargo; i++)
        {
            var newSlotInstance = Instantiate(slotDummy, slotDummy.transform.parent);
            newSlotInstance.gameObject.SetActive(true);
            newSlotInstance.transform.Translate(i * cargoSlotxOffset, 0f,0f);
            newSlotInstance.AssignFromConfig(cargos.All.PickRandom());
        }
    }
    private void OnStationExit()
    {
        //delete cargo taht was left
    }
}

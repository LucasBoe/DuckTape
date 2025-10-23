using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class MaintenanceUI : StationUI
{
    [SerializeField] private TextMeshProUGUI repairCostTmp;
    [SerializeField, ReadOnly] private int repairCost;

    private void OnEnable()
    {
        LoopEventHandler.Instance.OnStationEnterEvent.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        LoopEventHandler.Instance.OnStationEnterEvent.RemoveListener(UpdateUI);
    }

    void UpdateUI()
    {
        repairCost = (int) (DriveHandler.Instance.Train.missingTrainHP * GlobalBalancing.Value.RepairCostByMissingHealth);
        repairCostTmp.text = "Repair: (" + repairCost + "$)";
    }
    public void TryRepairTrain()
    {
        if (MoneyHandler.Instance.Money < repairCost)
            return;

        MoneyHandler.Instance.ChangeMoney(-repairCost);
        DriveHandler.Instance.Train.RepairTrain();

        UpdateUI();
    }

}

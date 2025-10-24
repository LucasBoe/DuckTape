using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.UI;

public class MaintenanceUI : StationUI
{
    [SerializeField] private TextMeshProUGUI repairCostTmp;
    [SerializeField] private Button repairButton;
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

        if (repairCost == 0)
        {
            repairCostTmp.text = "No repair needed";
            repairButton.interactable = false;
        }
        else
        {
            repairCostTmp.text = "Repair: (" + repairCost + "$)";
            repairButton.interactable = true;
        }
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

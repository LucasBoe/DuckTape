using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.UI;

public class MaintenanceUI : StationUI
{
    [SerializeField] private Button repairButton, coalButton, sandButton;
    private TextMeshProUGUI repairCostTmp, coalCostTmp, sandCostTmp;


    [SerializeField, ReadOnly] private int repairCost;
    private int sandCost;
    private int coalCost;

    void Awake()
    {
        repairCostTmp = repairButton.GetComponentInChildren<TextMeshProUGUI>();
        coalCostTmp = coalButton.GetComponentInChildren<TextMeshProUGUI>();
        sandCostTmp = sandButton.GetComponentInChildren<TextMeshProUGUI>();
    }

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
        //Update StationHandler and disable buttons that are not available - needs implementation

        //Get needed values
        repairCost = (int) (DriveHandler.Instance.Train.missingTrainHP * GlobalBalancing.Value.RepairCostByMissingHealth);
        coalCost = GlobalBalancing.Value.CoalCost;
        sandCost = GlobalBalancing.Value.SandCost;

        //Repair Cost Button 
        if (repairCost == 0)
        {
            repairCostTmp.text = "No repair needed";
            repairButton.gameObject.SetActive(false);
        }
        else
        {
            repairCostTmp.text = "Repair: (" + repairCost + "$)";
            repairButton.gameObject.SetActive(true);
        }

        //Buy Coal Button
        coalCostTmp.text = "Buy coal: (" + coalCost + "$)";

        //Buy Sand Button
        sandCostTmp.text = "Buy sand: (" + sandCost + "$)";


    }
    public void TryRepairTrain()
    {
        if (MoneyHandler.Instance.Money < repairCost)
            return;

        MoneyHandler.Instance.ChangeMoney(-repairCost);
        DriveHandler.Instance.Train.RepairTrain();

        UpdateUI();
    }

    public void TryBuyCoal()
    {
        if (DriveHandler.Instance.Engine.Coal >= DriveHandler.Instance.Engine.EngineConfig.MaxCoalStorage)
            return;

        if (MoneyHandler.Instance.TryChangeMoney(coalCost))
        {
            DriveHandler.Instance.Engine.Coal++;
        }
            
    }

    public void TryBuySand()
    {
        if (DriveHandler.Instance.Engine.Sand >= DriveHandler.Instance.Engine.EngineConfig.MaxSandStorage)
            return;

        if (MoneyHandler.Instance.TryChangeMoney(sandCost))
        {
            DriveHandler.Instance.Engine.Sand++;
        }
    }

}

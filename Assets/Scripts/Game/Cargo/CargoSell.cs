using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event = SS.Event;

public class CargoSell : CargoSlot
{
    [SerializeField] private SpriteRenderer backgroundImage;
    [SerializeField] private CargoConfigContainer cargos;
    
    public override bool TryAssign(Cargo instance)
    {
        if (!TryFetchPrice(instance.Config, out var sellPrice))
            return false;
        
        if (instance.OriginStationID == StatTracker.Instance.NumberOfStationsVisited)
            return false;
        
        Destroy(instance.gameObject);
        MoneyHandler.Instance.ChangeMoney(sellPrice, transform.position);
        return true;
    }
    private bool TryFetchPrice(CargoConfigBase config, out int price)
    {
        price = config.Value;
        return true;
    }
    private void OnMouseEnter()
    {
        if (!DragHandler.Instance.IsDragging)
            return;

        bool isSellable = DragHandler.Instance.CheckIsDraggingSellable();
        backgroundImage.color = isSellable ? Color.green : Color.red;
    }
    void OnMouseExit()
    {
        backgroundImage.color = Color.gray;
    }
}

[System.Serializable]
public class CargoPricePair
{
    public CargoConfigBase Config;
    public int Price;
}
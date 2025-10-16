using System.Collections.Generic;
using UnityEngine;
using Event = SS.Event;

public class CargoSell : CargoSlot
{
    [SerializeField] public List<CargoPricePair> SellPrices;
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
        foreach (var pair in SellPrices)
        {
            if (pair.Config == config)
            {
                price = pair.Price;
                return true;
            }
        }
        price = 0;
        return false;
    }
}

[System.Serializable]
public class CargoPricePair
{
    public CargoConfigBase Config;
    public int Price;
}
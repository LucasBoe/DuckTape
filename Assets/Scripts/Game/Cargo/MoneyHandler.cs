using SS;
using UnityEngine;

    public class MoneyHandler : Singleton<MoneyHandler>
{
    private int money;
    public int Money => money;
    
    public Event<int, Vector3> MoneyAddedEvent = new(), MoneyRemovedEvent = new();
    public Event<int> MoneyChangedEvent = new();

    public void ChangeMoney(int moneyAdded, Vector3 optWorldPosition = default)
    {
        money += moneyAdded;
        (moneyAdded > 0 ? MoneyAddedEvent : MoneyRemovedEvent)?.Invoke(moneyAdded, optWorldPosition);
        MoneyChangedEvent.Invoke(money);
    }

    public bool TryChangeMoney(int moneyAdded, Vector3 optWorldPosition = default)
    {
        if (money + moneyAdded < 0)
            return false;

        money += moneyAdded;
        (moneyAdded > 0 ? MoneyAddedEvent : MoneyRemovedEvent)?.Invoke(moneyAdded, optWorldPosition);
        MoneyChangedEvent.Invoke(money);

        return true;
    }
}
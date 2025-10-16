using System;
using SS;
using TMPro;
using UnityEngine;

public class MoneyUIHandler : MonoBehaviour, IDelayedStartObserver
{
    [SerializeField] private TMP_Text moneyText;
    public void DelayedStart()
    {
        Refresh(MoneyHandler.Instance.Money);
    }
    private void OnEnable() => MoneyHandler.Instance.MoneyChangedEvent.AddListener(Refresh);
    private void OnDisable() => MoneyHandler.Instance.MoneyChangedEvent.RemoveListener(Refresh);
    private void Refresh(int newMoney)
    {
        moneyText.text = $"{newMoney} $";
    }
}

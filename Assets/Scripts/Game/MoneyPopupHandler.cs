using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyPopupHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text dummy;
    [SerializeField] private Transform target;
    private void Awake()
    {
        dummy.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        MoneyHandler.Instance.MoneyAddedEvent.AddListener(OnMoneyAdded);
    }
    private void OnDisable()
    {
        MoneyHandler.Instance.MoneyAddedEvent.RemoveListener(OnMoneyAdded);
    }
    private void OnMoneyAdded(int amountChange, Vector3 worldPosition)
    {
        Debug.Log("OnMoneyAdded");
        var instance = Instantiate(dummy.gameObject, dummy.transform.parent).GetComponent<TMP_Text>();
        instance.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        instance.gameObject.SetActive(true);
        instance.text = $"+{amountChange}$";
        instance.transform.DOMove(target.position, 0.5f).SetEase(Ease.InBack);
        Destroy(instance.gameObject, 0.5f);
    }
}

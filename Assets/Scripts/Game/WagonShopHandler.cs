using System;
using System.Collections.Generic;
using UnityEngine;

public class WagonShopHandler : MonoBehaviour
{
    [SerializeField] private List<WagonConfigBase> forSale = new();
    [SerializeField] private WagonShopWagon shopWagonDummy;

    private void Awake()
    {
        shopWagonDummy.gameObject.SetActive(false);
    }
    private void Start()
    {
        float xOffset = 0f;
        foreach (var wagon in forSale)
        {
            xOffset += wagon.Length;
            var instance = Instantiate(shopWagonDummy, shopWagonDummy.transform.parent);
            instance.gameObject.SetActive(true);
            instance.Apply(wagon);
            instance.transform.Translate(xOffset, 0, 0);
        }
    }
}

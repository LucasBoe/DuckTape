using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WagonShopWagonUI : HoverUIBase<WagonShopWagon>
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button priceButton;
    protected override void Populate()
    {
        nameText.text = Source.Config.name;
        priceText.text = $"{Source.Config.ShopPrice}";
        priceButton.onClick.AddListener(TryBuy);
    }
    protected override void Update()
    {
        base.Update();
        priceButton.image.color = MoneyHandler.Instance.Money >= Source.Config.ShopPrice ? Color.green : Color.red;
    }
    private void TryBuy()
    {
        if (MoneyHandler.Instance.Money < Source.Config.ShopPrice)
            return;
        
        MoneyHandler.Instance.ChangeMoney(-Source.Config.ShopPrice);
        var train = FindObjectOfType<Train>();
        train.AppendFromConfig(Source.Config);
        Destroy(Source.gameObject);
    }
}

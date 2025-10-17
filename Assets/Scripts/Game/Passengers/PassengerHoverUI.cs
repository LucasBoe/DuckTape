using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassengerHoverUI : HoverUIBase<Passenger>
{
    [SerializeField] private Image stateTintImage;
    [SerializeField] private TMP_Text distanceAndPriceLabel, stateLabel;
    [SerializeField] private Button selectPassengerButton;
    protected override void Populate()
    {
        bool isAtStation = Source.State == PassengerState.Station;
        
        stateTintImage.gameObject.SetActive(isAtStation);
        stateLabel.text = "WAITING";
        
        string price = isAtStation
            ? $"{Source.passengerTravelDistanceToTicketPriceCurve.Evaluate(Source.StationsLeft)} $"
            : "";
        distanceAndPriceLabel.text = $"({Source.StationsLeft}) {price}";
        selectPassengerButton.gameObject.SetActive(isAtStation);
        selectPassengerButton.onClick.AddListener(() => Source.TrySelectForTrain());
    }
}
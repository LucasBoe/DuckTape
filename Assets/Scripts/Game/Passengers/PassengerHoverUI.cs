using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassengerHoverUI : HoverUIBase<Passenger>
{
    [SerializeField] private TMP_Text textLabel;
    [FormerlySerializedAs("button")] [SerializeField] private Button selectPassengerButton;
    protected override void Populate()
    {
        bool isAtStation = Source.State == PassengerState.Station;
        string price = isAtStation
            ? $"{Source.passengerTravelDistanceToTicketPriceCurve.Evaluate(Source.StationsLeft)} $"
            : "";
        textLabel.text = $"({Source.StationsLeft}) {price}";
        selectPassengerButton.gameObject.SetActive(isAtStation);
        selectPassengerButton.onClick.AddListener(() => Source.TrySelectForTrain());
    }
}
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum PassengerState
{
    Station,
    Train
}
public class Passenger : HoverableMonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField, BoxGroup("Balancing")] private int minStationsToTravel, maxStationsToTravel;
    [SerializeField, BoxGroup("Balancing")] public AnimationCurve passengerTravelDistanceToTicketPriceCurve;

    public int StationsLeft = 0;
    public PassengerState State = PassengerState.Station;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        StationsLeft = Random.Range(minStationsToTravel, maxStationsToTravel);
    }
    public const float PASSENGER_WIDTH = 4/16f;
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position +new Vector3(PASSENGER_WIDTH / 2f, .25f,0 ), new Vector3(PASSENGER_WIDTH, .5f));
    }
    public bool TrySelectForTrain()
    {
        State = PassengerState.Train;
        return true;
    }
}
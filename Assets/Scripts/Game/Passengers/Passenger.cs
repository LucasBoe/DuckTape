using System.Collections;
using System.Collections.Generic;
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
    public void Init()
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
        if (PassengerHandler.Instance.TryEnterTrain(this))
        {
            State = PassengerState.Train;
            return true;
        }
        return false;
    }

    public void AssignWagon(PassengerWagon wagon)
    {
        StartCoroutine(EnterWagonRoutine(wagon));
    }

    private IEnumerator EnterWagonRoutine(PassengerWagon wagon)
    {
        while (Mathf.Abs(wagon.transform.position.x - transform.position.x) > .01f)
        {
            var target = new Vector3(wagon.transform.position.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10f);
            yield return null;
        }
        wagon.Enter(this);
    }
}
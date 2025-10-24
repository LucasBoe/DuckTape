using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using SS;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Train : MonoBehaviour
{
    private List<TrainWagonSlot> slots = new();
    private List<Tweener> activeShakes = new();
    public const float WAGON_DISTANCE = 1f / 16f;
    [SerializeField] public CinemachineCamera Camera;

    //Train Damage
    [FormerlySerializedAs("trainHP")] [SerializeField] private float maxTrainHP = 2000;
    private float currentTrainHP;
    public float missingTrainHP => maxTrainHP - currentTrainHP;
    public Event<float> TrainDamageEvent = new();

    [SerializeField] private GameObject trainHealthBar;

    private void Awake()
    {
        currentTrainHP = maxTrainHP;
        Damage(0);
    }

    private void Start()
    {
        InvokeRepeating("CheckForDamage", 1, 1);
    }

    public void RepairTrain()
    {

    }
    public void AppendFromConfig(WagonConfigBase wagon)
    {
        //find new slot x
        var x = CalculateTrainLength();

        //create new slot
        GameObject newSlotGameObject = new GameObject();
        newSlotGameObject.transform.parent = transform;
        newSlotGameObject.name = slots.Count.ToString();
        newSlotGameObject.transform.position = new Vector3(x, 0, 0);
        TrainWagonSlot newSlot = newSlotGameObject.AddComponent<TrainWagonSlot>();
        slots.Add(newSlot);
        
        var newWagonInstance = WagonSpawner.Instance.SpawnWagon(wagon, newSlot);
        if (newWagonInstance is Engine engine)
            DriveHandler.Instance.ModifyEngine(engine);
    }

    private float CalculateTrainLength()
    {
        float x = 0f;
        foreach (var slot in slots)
        {
            if (!slot.WagonInstance)
                continue;

            x -= slot.WagonInstance.Config.Length;
            x -= WAGON_DISTANCE;
        }

        return x;
    }
    public int CalculateTotalWeight()
    {
        int totalWeight = 0;
        foreach (var slot in slots)
            totalWeight += slot.WagonInstance.CalculateWeight();
        
        return totalWeight;
    }
    public void TryShakeWagonsFor(float shakeStrengthAtCurrentSpeed, float duration)
    {
        foreach (var shake in activeShakes)
            shake.Rewind();
        
        activeShakes.Clear();
        foreach (var slot in slots)
        {
            if (!slot.WagonInstance)
                continue;
            
            activeShakes.Add(slot.WagonInstance.transform.DOShakePosition(duration, Vector3.up * shakeStrengthAtCurrentSpeed, vibrato: 100, fadeOut: false));
        }
    }

    private void CheckForDamage()
    {
        trainHealthBar.transform.parent.gameObject.SetActive(currentTrainHP < maxTrainHP);

        float currentSpeed = DriveHandler.Instance.Speed;
        float maxSpeed = ((EngineWagonConfig)DriveHandler.Instance.Engine.Config).MaxSpeed;
        
        if (currentSpeed > maxSpeed)
        {
            Damage((currentSpeed - maxSpeed) * 10f);
        }
        else
            trainHealthBar.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Damage(float amount)
    {
        currentTrainHP -= amount;
        trainHealthBar.transform.localScale = new Vector3(currentTrainHP / maxTrainHP, 1, 1);
        trainHealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        
        TrainDamageEvent?.Invoke(amount);

        if (currentTrainHP < 0)
            Debug.LogWarning("Train engine is destroyed.");
    }
}
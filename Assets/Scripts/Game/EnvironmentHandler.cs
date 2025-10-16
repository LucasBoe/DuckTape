
using UnityEngine;
using UnityEngine.Serialization;
using Event = SS.Event;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(0,100)] private float currentSpeed => DriveHandler.Instance.Speed;
    [SerializeField] IEnvironmentAsset[] spawners;

    public const float ASSET_AREA_RADIUS = 20f;
    
    [FormerlySerializedAs("OnBeginDrive")] public Event BeginDriveEvent = new();
    [FormerlySerializedAs("OnEndDrive")] public Event EndDriveEvent = new();

    private void OnValidate()
    {
        spawners = GetComponentsInChildren<IEnvironmentAsset>();
        foreach (var spawner in spawners)
        {
            spawner.Connect(this);
        }
    }
    private void Update()
    {
        if (currentSpeed == 0f)
            return;
        
        float translation = currentSpeed * Time.deltaTime;

        foreach (var spawner in spawners)
            spawner.Refresh(translation);
    }
}
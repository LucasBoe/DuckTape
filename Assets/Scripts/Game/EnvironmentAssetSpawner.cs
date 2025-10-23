using System.Collections.Generic;
using NaughtyAttributes;
using SS;
using UnityEngine;
using UnityEngine.Serialization;

public enum SpawnCondition
{
    Allways,
    OutsideOfStation,
    InsideStation,
}

public class EnvironmentAssetSpawner : MonoBehaviour, IEnvironmentAsset
{
    [SerializeField] public bool DoSpawn = true;
    [SerializeField] public SpawnCondition Condition = SpawnCondition.OutsideOfStation;

    [FormerlySerializedAs("assetRefs")] [SerializeField] private Transform[] assetDummys;
    
    [SerializeField] private bool limitXDistance = false;
    [SerializeField] private bool limitCount = false;
    [SerializeField] private bool limitByTrainSpeed = false;
    [SerializeField] private bool limitByProbabilityCurve = false;

    [SerializeField, ShowIf("limitXDistance")] private float minXDistance;
    [SerializeField, ShowIf("limitCount")] private float maxCount;
    [SerializeField, ShowIf("limitByTrainSpeed")] private float maxTrainSpeed;
    [SerializeField, ShowIf("limitByProbabilityCurve")] private ProbabilityCurveID probabilityCurve;
    
    
    private List<Transform> assets = new();
    private EnvironmentHandler handler;
    private int lastProbabilityEvaluation = 0;

    private void OnValidate()
    {
        var list = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
            list.Add(transform.GetChild(i));
        assetDummys = list.ToArray();
    }

    private void Awake()
    {
        foreach (var dummy in assetDummys)
        {
            dummy.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        handler.BeginDriveEvent.AddListener(OnBeginDrive);
        handler.EndDriveEvent.AddListener(OnEndDrive);
    }
    private void OnDisable()
    {
        handler.BeginDriveEvent.RemoveListener(OnBeginDrive);
        handler.EndDriveEvent.RemoveListener(OnEndDrive);
    }
    private void OnBeginDrive() => DoSpawn = Condition == SpawnCondition.Allways || Condition == SpawnCondition.OutsideOfStation;
    private void OnEndDrive() => DoSpawn = Condition == SpawnCondition.Allways || Condition == SpawnCondition.InsideStation;
    public void Refresh(float translation)
    {
        List<GameObject> toDestroy = new List<GameObject>();
        
        float smallestXPositionElement = TranslateAllAssets(translation, ref toDestroy);
        
        DeleteAccess(toDestroy);

        if (!DoSpawn)
            return;
        
        TrySpawnNewAsset(smallestXPositionElement);
    }

    public void Connect(EnvironmentHandler handler)
    {
        this.handler = handler;
    }
    private float TranslateAllAssets(float translation, ref List<GameObject> toDestroy)
    {
        float smallestXPositionElement = -EnvironmentHandler.ASSET_AREA_RADIUS;
        
        foreach (var asset in assets)
        {
            asset.position -= new Vector3(translation, 0f, 0f);
            smallestXPositionElement = Mathf.Max(smallestXPositionElement, asset.position.x);
            
            if (asset.position.x < -EnvironmentHandler.ASSET_AREA_RADIUS)
            {
                toDestroy.Add(asset.gameObject);
            }
        }

        return smallestXPositionElement;
    }

    private void DeleteAccess(List<GameObject> toDestroy)
    {
        for (var index = toDestroy.Count - 1; index >= 0; index--)
        {
            Destroy(toDestroy[index]);
            assets.Remove(toDestroy[index].transform);
        }
    }

    private void TrySpawnNewAsset(float smallestXPositionElement)
    {
        if (limitXDistance && smallestXPositionElement > EnvironmentHandler.ASSET_AREA_RADIUS - minXDistance)
            return;
        
        if (limitCount && assets.Count >= maxCount)
            return;
        
        if (limitByTrainSpeed && DriveHandler.Instance.Speed > maxTrainSpeed)
            return;

        if (limitByProbabilityCurve)
        {
            int currentProbabilityEvaluation = Mathf.CeilToInt(DriveHandler.Instance.Progression * DriveHandler.Instance.TotalDistance);
            if (currentProbabilityEvaluation <= lastProbabilityEvaluation)
                return;

            lastProbabilityEvaluation = currentProbabilityEvaluation;
            
            float probability = DriveHandler.Instance.CurrentSection.GetProbability(probabilityCurve, DriveHandler.Instance.Progression);
            if (Random.Range(0f, 1f) > probability)
                return;
        }

        var dummy = (Transform)(assetDummys.PickRandom());
        var spawnPos = new Vector3(EnvironmentHandler.ASSET_AREA_RADIUS, dummy.transform.position.y, 0f);
        var newAsset = Instantiate(dummy, spawnPos, Quaternion.identity);
        newAsset.gameObject.SetActive(true);
        assets.Add(newAsset);
    }
}

public interface IEnvironmentAsset
{
    public void Refresh(float translation);
    void Connect(EnvironmentHandler handler);
}

public enum ProbabilityCurveID
{
    Trees
}
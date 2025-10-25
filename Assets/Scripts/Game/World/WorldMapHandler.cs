using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using SS;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

[SingletonSettings(SingletonLifetime.Scene, _eager: true)]
public class WorldMapHandler : SingletonBehaviour<WorldMapHandler>, IDelayedStartObserver
{
    [SerializeField] CinemachineCamera camera;
    [SerializeField, ReadOnly] bool isVisible;

    [SerializeField] private WorldMapNode nodeDummy;
    [SerializeField] private WorldMapConnector connectorDummy;

    [FormerlySerializedAs("sections")] [SerializeField]
    private SectionContainer sectionPool;

    [FormerlySerializedAs("stations")] [SerializeField]
    private StationContainer stationPool;

    [SerializeField] private bool test;

    private List<WorldMapNode> stations = new();
    private List<WorldMapConnector> sections = new();
    
    private WorldMapNode selectedNode;
    private WorldMapConnector selectedConnector;
    public bool IsVisible => isVisible;
    protected override void Awake()
    {
        base.Awake();
        
        nodeDummy.gameObject.SetActive(false);
        connectorDummy.gameObject.SetActive(false);   
        //InitializeMap
        InitializeMap();
    }   
    public void DelayedStart()
    {
        if (test)
            Invoke(nameof(Show), .1f);
    }
    public WorldMapNode PickStartStation()
    {
        return stations.First();
    }
    [Button]
    public void Show()
    {
        camera.enabled = true;
        isVisible = true;
        GamePhaseHandler.Instance.AddGamePhaseOverride(GamePhase.Map);
        Refresh();
    }
    private void Refresh()
    {
        foreach (var station in stations)
        {
            station.Refresh();
        }

        foreach (var section in sections)
        {
            section.Refresh();
        }
    }
    [Button]
    public void Hide()
    {
        camera.enabled = false;
        isVisible = false;
        GamePhaseHandler.Instance.RemoveGamePhaseOverride(GamePhase.Map);
    }

    private void InitializeMap()
    {
        SpawnStations();
        SpawnSections();
        AdaptCamera();
    }
    private void SpawnStations()
    {
        int numberOfStationsToSpawn = 12;
        for (int i = 0; i < numberOfStationsToSpawn; i++)
        {
            var config = stationPool.All.PickRandom();
            var spawnPos = Vector2Int.zero;

            while (stations.Count > 0 && (DistanceToNextExistingStations(spawnPos) < 4f ||
                                          DistanceToNextExistingStations(spawnPos) > 8f))
                spawnPos = GetRandomStationPosition();

            var instance = Instantiate(nodeDummy, nodeDummy.transform.parent);
            instance.transform.localPosition = (Vector2)spawnPos;
            instance.gameObject.SetActive(true);
            instance.Apply(config);
            stations.Add(instance);
        }

        float DistanceToNextExistingStations(Vector2Int pos)
        {
            return stations.Select(s => Vector2.Distance(pos, s.transform.localPosition)).Min();
        }

        Vector2Int GetRandomStationPosition()
        {
            int maxXSize = 100;
            int maxYSize = 20;
            return new Vector2Int(UnityEngine.Random.Range(0, maxYSize),
                UnityEngine.Random.Range(-maxYSize / 2, maxYSize / 2));
        }
    }

    private void SpawnSections()
    {
        foreach (var station in stations)
        {
            int numberOfConnections = station.Config.Type switch
            {
                StationType.Residential => 3,
                StationType.City => 5,
                _ => 2
            };

            List<WorldMapNode> neightbours = GetNeightbours(station, numberOfConnections);
            foreach (var neightbour in neightbours)
            {
                if (ConnectionExists(station, neightbour))
                    continue;

                if (ConnectionIntersects(station, neightbour))
                    continue;


                var instance = Instantiate(connectorDummy, connectorDummy.transform.parent);
                instance.Init(sectionPool.All.PickRandom());
                instance.Connect(station, neightbour);
                instance.gameObject.SetActive(true);
                sections.Add(instance);
            }
        }

        bool ConnectionIntersects(WorldMapNode start, WorldMapNode end)
        {
            var line1 = WorldMapConnector.Crop(start.transform.position, end.transform.position);
            
            foreach (var section in sections)
            {
                var line2 = WorldMapConnector.Crop(section.Start.transform.position, section.End.transform.position);
                if (LineUtil.SegmentsIntersect(line1.Item1, line1.Item2, line2.Item1, line2.Item2))
                    return true;
            }
            
            return false;
        }
    }
    [Button]
    private void AdaptCamera()
    {
        Vector2 cameraPos = Vector2.zero;
        foreach (var station in stations)
            cameraPos += (Vector2)station.transform.position;        
        cameraPos /= stations.Count;
        
        camera.transform.position = new Vector3(cameraPos.x, cameraPos.y, -10f);
    }
    public bool ConnectionExists(WorldMapNode a, WorldMapNode b)
    {
        foreach (var section in sections)
        {
            if ((section.Start == a && section.End == b) || (section.Start == b && section.End == a))
                return true;
        }

        return false;
    }
    public bool TryGetConnection(WorldMapNode a, WorldMapNode b, out WorldMapConnector connection)
    {
        foreach (var section in sections)
        {
            if ((section.Start == a && section.End == b) || (section.Start == b && section.End == a))
            {
                connection = section;
                return true;
            }
        }

        connection = null;
        return false;
    }
    public bool IsSectionConnectedTo(WorldMapConnector section, WorldMapNode node)
    {
        if (section.Start == node || section.End == node)
            return true;
        
        return false;
    }    
    private List<WorldMapNode> GetNeightbours(WorldMapNode station, int numberOfConnections)
    {
        List<WorldMapNode> neightbours = new();
        var allSorted = stations
            .OrderBy(s => Vector2.Distance(station.transform.localPosition, s.transform.localPosition)).ToArray();
        for (int i = 1; i < numberOfConnections; i++) //starting at 1 removes selfreference
            neightbours.Add(allSorted[i]);

        return neightbours;
    }
    public void TrySelect(WorldMapNode node)
    {
        if (TryGetConnection(StationHandler.Instance.CurrentStation, node, out WorldMapConnector connector))
        {
            TrySelect(connector, node);
        }
    }
    public void TrySelect(WorldMapConnector connector)
    {
        var node = connector.Start == StationHandler.Instance.CurrentStation ? connector.End : connector.Start;
        TrySelect(connector, node);
    }    
    private void TrySelect(WorldMapConnector connector, WorldMapNode node)
    {
        if (DriveHandler.Instance.Progression > 0f)
            return;
        
        if (selectedNode)
            selectedNode.SetSelected(false);

        if (selectedConnector)
            selectedConnector.SetSelected(false);
        
        selectedNode = node;
        selectedConnector = connector;

        DriveHandler.Instance.ModifySection(selectedConnector.Section);
        StationHandler.Instance.ModifyTargetStation(selectedNode);
        selectedNode.SetSelected(true);
        selectedConnector.SetSelected(true);
    }
}

public interface ISelectableWorldMapElement
{
    void SetSelected(bool selected);
}
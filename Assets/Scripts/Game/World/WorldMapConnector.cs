using System;
using SS;
using TMPro;
using UnityEngine;

public class WorldMapConnector : MonoBehaviour
{
    public WorldMapNode Start;
    public WorldMapNode End;
    public float Distance;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private TMP_Text distanceLabelText;
    public void Connect(WorldMapNode start, WorldMapNode end)
    {
        Start = start;
        End = end;
        
        
        Vector2 a = start.transform.position;
        Vector2 b = end.transform.position;
        
        Distance = Vector3.Distance(a, b);

        var copped = Crop(a, b);
        
        transform.position = Vector2.Lerp(a, b, 0.5f);
        transform.right = (a - b).normalized;
        
        lineRenderer.SetPosition(0, copped.Item1);
        lineRenderer.SetPosition(1, copped.Item2);
        
        distanceLabelText.text = $"{Mathf.Round(Distance * 10f)/10f} km";
        if (transform.rotation.eulerAngles.z > 90 || transform.rotation.eulerAngles.z < -90f)
            distanceLabelText.transform.localRotation = Quaternion.Euler(0, 0, 180f);
            
    }
    public static Tuple<Vector2, Vector2> Crop(Vector2 a, Vector2 b, float cropDistance = .6f)
    {
        return new Tuple<Vector2, Vector2>(Vector2.MoveTowards(a, b, .6f), Vector2.MoveTowards(b, a, .6f));
    }
}
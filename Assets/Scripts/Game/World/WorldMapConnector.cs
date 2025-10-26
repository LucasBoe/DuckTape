using System;
using NaughtyAttributes;
using SS;
using TMPro;
using UnityEngine;

public class WorldMapConnector : MonoBehaviour, ISelectableWorldMapElement
{
    public WorldMapNode Start;
    public WorldMapNode End;
    public Section Section;
    public float Distance;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private TMP_Text distanceLabelText;
    [SerializeField] private SpriteRenderer iconDummy;
    [SerializeField] private BoxCollider2D collider;

    [SerializeField, ReadOnly] private bool isSelected;

    private void Awake()
    {
        iconDummy.gameObject.SetActive(false);
    }

    public void Init(Section sectionConfig)
    {
        Section = sectionConfig;
    }
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

        collider.size = new Vector2(Distance, .1f);

    }
    public static Tuple<Vector2, Vector2> Crop(Vector2 a, Vector2 b, float cropDistance = .6f)
    {
        return new Tuple<Vector2, Vector2>(Vector2.MoveTowards(a, b, .6f), Vector2.MoveTowards(b, a, .6f));
    }

    public void Refresh()
    {
        bool isAvailable = WorldMapHandler.Instance.IsSectionConnectedTo(this, StationHandler.Instance.CurrentStation);
        lineRenderer.startColor = isAvailable ? Color.white : Color.gray;    
        lineRenderer.endColor = isAvailable ? Color.white : Color.gray;
        distanceLabelText.gameObject.SetActive(isAvailable);
        collider.enabled = isAvailable;
    }
    private void OnMouseDown()
    {
        WorldMapHandler.Instance.TrySelect(this);
    }
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        lineRenderer.widthMultiplier = isSelected ? .3f : .1f;
        iconDummy.transform.parent.DestroyAllChildren(iconDummy.transform);
        if (selected)
        {
            foreach (var icon in Section.GetIcons())
            {
                var instance = Instantiate(iconDummy, iconDummy.transform.parent);
                instance.sprite = icon.Item1;
                float l = icon.Item2;
                float r = Distance / 2f - .6f;
                instance.transform.localPosition = new Vector3(Mathf.Lerp(-r, r, l),0,0);
                instance.gameObject.SetActive(true);
            }
        }
    }
}
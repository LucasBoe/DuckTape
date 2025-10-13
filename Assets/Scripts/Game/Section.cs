using NaughtyAttributes;
using SS;
using UnityEngine;

public class Section : ContaineableScriptableObject
{
    [CurveRange(0, -45, 1, 45)] public AnimationCurve SlopeOverSection;
    [CurveRange(0, 0, 1, 1)] public AnimationCurve TreesOverSection;
    public int Length = 1000;
}
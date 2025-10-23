using NaughtyAttributes;
using SS;
using UnityEngine;

public class Section : ContaineableScriptableObject
{
    [CurveRange(0, -45, 1, 45)] public AnimationCurve SlopeOverSection;
    [CurveRange(0, 0, 1, 1)] public AnimationCurve TreesOverSection;
    public int Length = 1000;

    public float GetProbability(ProbabilityCurveID curveID, float t)
    {
        if (TryGetCurveByName(curveID, out var curve))
        {
            return curve.Evaluate(t);
        }
        
        return 0;
    }

    private bool TryGetCurveByName(ProbabilityCurveID curveID, out AnimationCurve curve)
    {
        switch (curveID)
        {
            case ProbabilityCurveID.Trees:
                curve = TreesOverSection;
                return true;
        }
        
        curve = null;
        return false;
    }
}
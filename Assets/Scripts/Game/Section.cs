using System;
using System.Collections;
using System.Collections.Generic;
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

    public List<Tuple<Sprite, float>> GetIcons()
    {
        List<Tuple<Sprite, float>> icons = new List<Tuple<Sprite, float>>();
        
        var slopes = AnimationCurveToFloatArray(SlopeOverSection);
        var high = GetClosestPoint(slopes, -100);
        var low = GetClosestPoint(slopes, 100);

        SectionContainer container = Container as SectionContainer;
        
        if (high.Item2 > 0)
            icons.Add(new Tuple<Sprite, float>(container.SlopeUpSprite, high.Item1));

        if (low.Item2 > 0)
            icons.Add(new Tuple<Sprite, float>(container.SlopeDownSprite, low.Item1));
        
        return icons;
    }
    private Tuple<float, float> GetClosestPoint(float[] slopes, float goal)
    {
        int lastX = -1;
        float lastY = 0f;

        for (var index = 0; index < slopes.Length; index++)
        {
            float currentY = slopes[index];
            if (Distance(currentY, goal) < Distance(lastY, goal))
            {
                lastX = index;
                lastY = currentY;
            }
        }
        
        return new Tuple<float, float>(lastX / 100f, lastY);

        float Distance(float x, float y)
        {
            return Mathf.Abs(x - y);
        }
    }
    private float[] AnimationCurveToFloatArray(AnimationCurve curve)
    {
        List<float> floats = new();
        for (int i = 0; i < 100; i++)
        {
            float f = i / 100f;
            floats.Add(curve.Evaluate(f));
        }
        return floats.ToArray();
    }
}
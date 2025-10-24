using UnityEngine;

public static class LineUtil
{
    // Tolerance for float comparisons (adjust if needed)
    private const float EPS = 1e-6f;

    // Public API: returns true if segments [p1,p2] and [q1,q2] intersect at any point.
    public static bool SegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2, float eps = EPS)
    {
        Vector2 r = p2 - p1;
        Vector2 s = q2 - q1;

        float rxs  = Cross(r, s);
        float qpxr = Cross(q1 - p1, r);

        // Handle degenerate segments (points)
        if (r.sqrMagnitude <= eps * eps && s.sqrMagnitude <= eps * eps)
            return (p1 - q1).sqrMagnitude <= eps * eps;
        if (r.sqrMagnitude <= eps * eps)
            return PointOnSegment(p1, q1, q2, eps);
        if (s.sqrMagnitude <= eps * eps)
            return PointOnSegment(q1, p1, p2, eps);

        // Parallel?
        if (Mathf.Abs(rxs) <= eps)
        {
            // Parallel but not collinear
            if (Mathf.Abs(qpxr) > eps) return false;

            // Collinear: check 1D overlap on r's parameter t
            float rr  = Vector2.Dot(r, r);
            float t0  = Vector2.Dot(q1 - p1, r) / rr;
            float t1  = t0 + Vector2.Dot(s, r) / rr;
            float tMin = Mathf.Min(t0, t1);
            float tMax = Mathf.Max(t0, t1);

            // Overlap if intervals intersect (allowing epsilon)
            return !(tMax < -eps || tMin > 1f + eps);
        }

        // Proper intersection: solve for parameters t and u
        float t = Cross(q1 - p1, s) / rxs;
        float u = Cross(q1 - p1, r) / rxs;

        return t >= -eps && t <= 1f + eps && u >= -eps && u <= 1f + eps;
    }

    // --- Helpers ---
    private static float Cross(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

    private static bool PointOnSegment(Vector2 p, Vector2 a, Vector2 b, float eps)
    {
        Vector2 ab = b - a;
        Vector2 ap = p - a;

        // Collinearity
        if (Mathf.Abs(Cross(ab, ap)) > eps) return false;

        // Within segment bounds (using dot)
        float dot = Vector2.Dot(ap, ab);
        if (dot < -eps) return false;
        return dot <= Vector2.Dot(ab, ab) + eps;
    }
}
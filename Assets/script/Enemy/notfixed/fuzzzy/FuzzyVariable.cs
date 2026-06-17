using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FuzzyVariable
{
    public string name;
    public float minVale;
    public float maxVale;
    public List<FuzzySet> fuzzySets = new List<FuzzySet>();

    public float GetMembership(string setName, float inpuValue)
    {
        foreach (var set in fuzzySets)
        {
            if (set.name == setName)
            {
                return set.CalculateMembership(inpuValue);
            }
        }
        return 0f;
    }
}


[Serializable]
public class FuzzySet
{
    public string name;
    public enum SetType { Triangular, Trapezoidal, Gaussian, Singleton, CustomPoint }
    public SetType type;

    public float a, b, c, d;

    // Custom 11-point parameters
    public Vector2[] customPoints = new Vector2[11]; // x = input value, y = membership (0-1)


    public float CalculateMembership(float x)
    {
        switch(type)
        {
            case SetType.Triangular:
                return MathF.Max(MathF.Min((x - a) / (b - a), (c - x) / (c - b)), 0);
            case SetType.Trapezoidal:
                return MathF.Max(MathF.Min(MathF.Min((x - a) / (b - a), 1), (d - x) / (d - c)), 0);
            case SetType.Gaussian:
                return MathF.Exp(-MathF.Pow(x - b, 2) / (2 * MathF.Pow(c, 2)));
            case SetType.CustomPoint:
                return Calculate11PointMembership(x);
            default:
                return 0f;
        }
    }

    private float Calculate11PointMembership(float x)
    {
        // Find the segment where x lies
        for (int i = 0; i < customPoints.Length - 1; i++)
        {
            if (x >= customPoints[i].x && x <= customPoints[i + 1].x)
            {
                // Linear interpolation between points
                float t = Mathf.InverseLerp(customPoints[i].x, customPoints[i + 1].x, x);
                return Mathf.Lerp(customPoints[i].y, customPoints[i + 1].y, t);
            }
        }
        return 0f; // Outside defined range
    }

}


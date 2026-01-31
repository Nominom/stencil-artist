using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ScoringRule
{
    public enum ScoringType
    {
        Closeness,
        Farawayness,
        BelowHorizon,
        AboveHorizon
    }

    public ScoringType Scoring;
    [Tooltip("Only applies to closeness and farawayness. Measured as fraction of the canvas (0.0-1.0). Inside this range you get either points (closeness) or minus points (farawayness)")]
    public float Range = 0.1f;
    public StencilTag TargetTag;
    [Tooltip("Alternatively you can target a single stencil by name.")]
    public string AltTargetName;
    public int RewardScore = 10;
}
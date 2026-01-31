using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoringMeowster : MonoBehaviour
{
    public int PlayerScore = 0;
    public int BaseScorePerStencil = 10;

    void OnStencilPainted(StencilScobj stencil)
    {
        // Score
        foreach(ScoringRule rule in stencil.scoringRules)
        {
            if (!string.IsNullOrEmpty(rule.AltTargetName))
            {

            }
        }
    }

    List<StencilScobj> GetNearbyStencilsByTag(StencilTag tag)
    {
        List<StencilScobj> results = new List<StencilScobj>();
        foreach (StencilScobj stencil in GetNearbyStencils())
        {
            if (stencil.tags.CompareTo(tag) == 1)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    List<StencilScobj> GetNearbyStencilsByName(string name)
    {
        List<StencilScobj> results = new List<StencilScobj>();
        foreach (StencilScobj stencil in GetNearbyStencils())
        {
            if (stencil.name == name)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    List<StencilObj> GetNearbyStencils(Vector2 pos)
    {

    }
}

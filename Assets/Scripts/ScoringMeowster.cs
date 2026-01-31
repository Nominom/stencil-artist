using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoringMeowster : MonoBehaviour
{
    public int PlayerScore = 0;
    public int BaseScorePerStencil = 10;

    void OnStencilPainted(StencilObj stencil)
    {
        // Score
        foreach(ScoringRule rule in stencil.data.scoringRules)
        {
            if (!string.IsNullOrEmpty(rule.AltTargetName))
            {
                PlayerScore += GetNearbyStencilsByName(rule.AltTargetName, stencil).Count * rule.RewardScore;
            }
            else
            {
                PlayerScore += GetNearbyStencilsByTag(rule.TargetTag, stencil).Count * rule.RewardScore;
            }
        }
    }

    List<StencilScobj> GetNearbyStencilsByTag(StencilTag tag, StencilObj baseStencil)
    {
        List<StencilScobj> results = new List<StencilScobj>();
        System.Collections.IList nearbyStencils = GetNearbyStencils(new Vector2(baseStencil.x, baseStencil.y));
        for (int i = 0; i < nearbyStencils.Count; i++)
        {
            StencilScobj stencil = (StencilScobj)nearbyStencils[i];
            if (stencil.tags.CompareTo(tag) == 1)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    List<StencilScobj> GetNearbyStencilsByName(string name, StencilObj baseStencil)
    {
        List<StencilScobj> results = new List<StencilScobj>();
        System.Collections.IList nearbyStencils = GetNearbyStencils(new Vector2(baseStencil.x, baseStencil.y));
        for (int i = 0; i < nearbyStencils.Count; i++)
        {
            StencilScobj stencil = (StencilScobj)nearbyStencils[i];
            if (stencil.name == name)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    List<StencilObj> GetNearbyStencils(Vector2 pos)
    {
        throw new NotImplementedException();
        // return StencilManager.Instance.GetNearbyStencils(pos);
    }
}

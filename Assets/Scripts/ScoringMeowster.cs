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
                switch (rule.Scoring)
                {
                    case ScoringRule.ScoringType.Closeness:
                        PlayerScore += GetNearbyStencilsByName(rule.AltTargetName, stencil).Count * rule.RewardScore;
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        PlayerScore -= GetNearbyStencilsByName(rule.AltTargetName, stencil).Count * rule.RewardScore;
                        break;
                    case ScoringRule.ScoringType.BelowHorizon:
                        if (stencil.y < GetPaintingCanvasSize().y / 2f)
                        {
                            PlayerScore += rule.RewardScore;
                        }
                        break;
                    case ScoringRule.ScoringType.AboveHorizon:
                        if (stencil.y > GetPaintingCanvasSize().y / 2f)
                        {
                            PlayerScore += rule.RewardScore;
                        }
                        break;
                    default:
                        Debug.LogWarning("Unrecognized scoring rule, implement scoring in this file");
                        break;
                }
            }
            else
            {
                switch (rule.Scoring)
                {
                    case ScoringRule.ScoringType.Closeness:
                        PlayerScore += GetNearbyStencilsByTag(rule.TargetTag, stencil).Count * rule.RewardScore;
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        PlayerScore -= GetNearbyStencilsByTag(rule.TargetTag, stencil).Count * rule.RewardScore;
                        break;
                    case ScoringRule.ScoringType.BelowHorizon:
                        if (stencil.y < GetPaintingCanvasSize().y / 2f)
                        {
                            PlayerScore += rule.RewardScore;
                        }
                        break;
                    case ScoringRule.ScoringType.AboveHorizon:
                        if (stencil.y > GetPaintingCanvasSize().y / 2f)
                        {
                            PlayerScore += rule.RewardScore;
                        }
                        break;
                    default:
                        Debug.LogWarning("Unrecognized scoring rule, implement scoring in this file");
                        break;
                }
            }
        }
    }

    Vector2 GetPaintingCanvasSize()
    {
        throw new NotImplementedException();
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

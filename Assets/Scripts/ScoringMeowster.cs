using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoringMeowster : MonoBehaviour
{
    public int PlayerScore = 0;
    public int BaseScorePerStencil = 10;

    CanvasPaintingScript canvasPainter;

    private void Awake()
    {
        canvasPainter = FindFirstObjectByType<CanvasPaintingScript>();
    }

    public void OnStencilPainted(StencilObj stencil)
    {
        // Score
        PlayerScore += BaseScorePerStencil;
        foreach(ScoringRule rule in stencil.data.scoringRules)
        {
            if (!string.IsNullOrEmpty(rule.AltTargetName))
            {
                switch (rule.Scoring)
                {
                    case ScoringRule.ScoringType.Closeness:
                        PlayerScore += GetNearbyStencilsByName(rule.AltTargetName, stencil, rule.Range).Count * rule.RewardScore;
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        PlayerScore -= GetNearbyStencilsByName(rule.AltTargetName, stencil, rule.Range).Count * rule.RewardScore;
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
                        PlayerScore += GetNearbyStencilsByTag(rule.TargetTag, stencil, rule.Range).Count * rule.RewardScore;
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        PlayerScore -= GetNearbyStencilsByTag(rule.TargetTag, stencil, rule.Range).Count * rule.RewardScore;
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

    List<StencilScobj> GetNearbyStencilsByTag(StencilTag tag, StencilObj baseStencil, float range)
    {
        List<StencilScobj> results = new List<StencilScobj>();
        System.Collections.IList nearbyStencils = GetNearbyStencils(new Vector2(baseStencil.x, baseStencil.y), range);
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

    List<StencilScobj> GetNearbyStencilsByName(string name, StencilObj baseStencil, float range)
    {
        List<StencilScobj> results = new List<StencilScobj>();
        System.Collections.IList nearbyStencils = GetNearbyStencils(new Vector2(baseStencil.x, baseStencil.y), range);
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

    List<StencilObj> GetNearbyStencils(Vector2 pos, float range)
    {
        List<StencilObj> stencils = new List<StencilObj>(canvasPainter.paintedStencils);
        List<StencilObj> results = new List<StencilObj>();
        foreach(StencilObj stencil in stencils) 
        {
            if (Vector2.Distance(new Vector2(stencil.x, stencil.y), pos) < range)
            {
                results.Add(stencil);
            }
        }

        return results;
    }
}

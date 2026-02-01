using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoringMeowster : MonoBehaviour
{
    public int PlayerScore = 0;
    public bool updateAllScores = true;
    public GameObject positiveEffectPrefab;
    public GameObject negativeEffectPrefab;
    
    CanvasPaintingScript canvasPainter;
    
    public AudioSource audioSource;
    public AudioClip positiveSound;
    public AudioClip negativeSound;
    public AudioClip neutralSound;

    private void Awake()
    {
        canvasPainter = FindFirstObjectByType<CanvasPaintingScript>();
    }

    public void OnStencilPainted(StencilObj stencil)
    {
        int oldScore = PlayerScore;
        // Score the new stencil
        UpdateScore(stencil);

        if (updateAllScores)
        {
            // Score old stencils
            foreach (var stencilObj in canvasPainter.paintedStencils)
            {
                UpdateScore(stencilObj);
            }
        }

        if (PlayerScore > oldScore)
        {
            audioSource.PlayOneShot(positiveSound);
        }else if (PlayerScore < oldScore)
        {
            audioSource.PlayOneShot(negativeSound);
        }
        else
        {
            audioSource.PlayOneShot(neutralSound);
        }

        FindFirstObjectByType<UIMEOW>().OnScoreChanged(PlayerScore);
    }

    void UpdateScore(StencilObj stencil)
    {
        int oldScore = stencil.scoreGiven;
        int newScore = 0;
        string scoringDesc = "";
        
        foreach(ScoringRule rule in stencil.data.scoringRules)
        {
            if (!string.IsNullOrEmpty(rule.AltTargetName))
            {
                switch (rule.Scoring)
                {
                    case ScoringRule.ScoringType.Closeness:
                        newScore += GetNearbyStencilsByName(rule.AltTargetName, stencil, rule.Range).Count * rule.RewardScore;
                        scoringDesc += "Scores " + rule.RewardScore + " for each " + rule.AltTargetName + " within " + rule.Range + " units.";
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        newScore -= GetNearbyStencilsByName(rule.AltTargetName, stencil, rule.Range).Count * rule.RewardScore;
                        scoringDesc += "Scores " + rule.RewardScore + " for each " + rule.AltTargetName + " within " + rule.Range + " units.";
                        break;
                    case ScoringRule.ScoringType.BelowHorizon:
                        if (stencil.y < 0.5f)
                        {
                            newScore += rule.RewardScore;
                            scoringDesc += "Scores " + rule.RewardScore + " for being below the horizon";
                        }
                        break;
                    case ScoringRule.ScoringType.AboveHorizon:
                        if (stencil.y > 0.5f)
                        {
                            newScore += rule.RewardScore;
                            scoringDesc += "Scores " + rule.RewardScore + " for being above the horizon";
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
                        newScore += GetNearbyStencilsByTag(rule.TargetTag, stencil, rule.Range).Count * rule.RewardScore;
                        scoringDesc += "Scores " + rule.RewardScore + " for each " + rule.TargetTag + " within " + rule.Range + " units.";
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        newScore += GetNearbyStencilsByTag(rule.TargetTag, stencil, rule.Range).Count == 0 ? 1 : 0 * rule.RewardScore;
                        scoringDesc += "Scores " + rule.RewardScore + " if near " + rule.TargetTag + ".";
                        break;
                    case ScoringRule.ScoringType.BelowHorizon:
                        if (stencil.y < 0.5f)
                        {
                            newScore += rule.RewardScore;
                            scoringDesc += "Scores " + rule.RewardScore + " for being below the horizon";
                        }
                        break;
                    case ScoringRule.ScoringType.AboveHorizon:
                        if (stencil.y > 0.5f)
                        {
                            newScore += rule.RewardScore;
                            scoringDesc += "Scores " + rule.RewardScore + " for being above the horizon";
                        }
                        break;
                    default:
                        Debug.LogWarning("Unrecognized scoring rule, implement scoring in this file");
                        break;
                }
            }
        }
        
        int scoreDelta = newScore - oldScore;
        PlayerScore += scoreDelta;
        stencil.scoreGiven = newScore;

        Quaternion effectRotation = canvasPainter.transform.rotation;

        if (scoreDelta > 0 && positiveEffectPrefab)
        {
            var inst = Instantiate(positiveEffectPrefab, stencil.worldPos, effectRotation);
            inst.GetComponent<ParticleSystem>()?.Emit(scoreDelta);
        }
        else if (scoreDelta < 0 && negativeEffectPrefab)
        {
            var inst = Instantiate(negativeEffectPrefab, stencil.worldPos, effectRotation);
            inst.GetComponent<ParticleSystem>()?.Emit(-scoreDelta);
        }
    }

    Vector2 GetPaintingCanvasSize()
    {
        return FindFirstObjectByType<CanvasPaintingScript>().canvasSize;
    }

    List<StencilObj> GetNearbyStencilsByTag(StencilTag tag, StencilObj baseStencil, float range)
    {
        List<StencilObj> results = new List<StencilObj>();
        List<StencilObj> nearbyStencils = GetNearbyStencils(baseStencil.position, range);
        for (int i = 0; i < nearbyStencils.Count; i++)
        {
            StencilObj stencil = nearbyStencils[i];
            
            if (stencil == baseStencil)
                continue;
            
            if ((stencil.data.tags & tag) != 0)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    List<StencilObj> GetNearbyStencilsByName(string name, StencilObj baseStencil, float range)
    {
        List<StencilObj> results = new List<StencilObj>();
        List<StencilObj> nearbyStencils = GetNearbyStencils(baseStencil.position, range);
        for (int i = 0; i < nearbyStencils.Count; i++)
        {
            StencilObj stencil = nearbyStencils[i];
            
            if (stencil == baseStencil)
                continue;
            
            if (stencil.data.name == name)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    List<StencilObj> GetNearbyStencils(Vector2 pos, float range)
    {
        List<StencilObj> results = new List<StencilObj>();
        foreach(StencilObj stencil in canvasPainter.paintedStencils)
        {
            float dist = Vector2.Distance(stencil.position, pos);
            if (dist < range)
            {
                results.Add(stencil);
            }
        }

        return results;
    }

    public string GetScoringDesc(StencilScobj stencil)
    {
        string scoringDesc = "";

        foreach (ScoringRule rule in stencil.scoringRules)
        {
            if (!string.IsNullOrEmpty(rule.AltTargetName))
            {
                switch (rule.Scoring)
                {
                    case ScoringRule.ScoringType.Closeness:
                        scoringDesc += "Scores " + rule.RewardScore + " for each " + rule.AltTargetName + " within " + rule.Range + " units. ";
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        scoringDesc += "Scores " + rule.RewardScore + " for each " + rule.AltTargetName + " within " + rule.Range + " units. ";
                        break;
                    case ScoringRule.ScoringType.BelowHorizon:
                        scoringDesc += "Scores " + rule.RewardScore + " for being below the horizon. ";
                        break;
                    case ScoringRule.ScoringType.AboveHorizon:
                        scoringDesc += "Scores " + rule.RewardScore + " for being above the horizon. ";
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
                        scoringDesc += "Scores " + rule.RewardScore + " for each " + rule.TargetTag + " within " + rule.Range + " units. ";
                        break;
                    case ScoringRule.ScoringType.Farawayness:
                        scoringDesc += "Scores " + rule.RewardScore + " if near " + rule.TargetTag + ". ";
                        break;
                    case ScoringRule.ScoringType.BelowHorizon:
                        scoringDesc += "Scores " + rule.RewardScore + " for being below the horizon. ";
                        break;
                    case ScoringRule.ScoringType.AboveHorizon:
                        scoringDesc += "Scores " + rule.RewardScore + " for being above the horizon. ";
                        break;
                    default:
                        Debug.LogWarning("Unrecognized scoring rule, implement scoring in this file");
                        break;
                }
            }
        }

        return scoringDesc;
    }
}

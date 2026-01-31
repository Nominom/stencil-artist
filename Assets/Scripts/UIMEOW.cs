using TMPro;
using UnityEngine;

public class UIMEOW : MonoBehaviour
{
    public TextMeshProUGUI StencilTitle;
    public TextMeshProUGUI StencilDescription;
    public TextMeshProUGUI ScoreText;

    public void OnStencilSelected(StencilScobj stencil)
    {
        StencilTitle.SetText(stencil.name);
        StencilDescription.SetText(stencil.description);
    }

    public void OnScoreChanged(int score)
    {
        ScoreText.SetText("Score: " + score.ToString());
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMEOW : MonoBehaviour
{
    public TextMeshProUGUI StencilTitle;
    public TextMeshProUGUI StencilDescription;
    public TextMeshProUGUI ScoreText;
    public Button FinishAndExportButton;
    public GameObject FinishedScreen;

    public void OnStencilSelected(StencilScobj stencil)
    {
        StencilTitle.SetText(stencil.name);
        string scoringText = FindFirstObjectByType<ScoringMeowster>().GetScoringDesc(stencil);
        StencilDescription.SetText(stencil.description + "\n" + scoringText);
        
    }

    public void OnScoreChanged(int score)
    {
        ScoreText.SetText("Score: " + score.ToString());
    }

    private void Awake()
    {
        FinishAndExportButton.onClick.AddListener(FinishAndExport);
    }

    private void FinishAndExport()
    {
        FinishedScreen.SetActive(true);

        // FindFirstObjectByType<CanvasPaintingScript>().canvasMaterial.mainTexture.IntoPNG().SaveAs();
    }
}

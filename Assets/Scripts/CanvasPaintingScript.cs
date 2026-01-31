using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasPaintingScript : MonoBehaviour
{
    public LayerMask paintLayer;
    public RenderTexture renderTexture;
    public Material canvasMaterial;
    MeshRenderer meshRenderer;

    [Range(1, 100)]
    public int radius = 10;

    Texture2D canvasTexture;

    private bool needUpdate = false;

    private Vector2 canvasHitCoord;
    public GameObject stencil;
    private Texture2D stencilTexture;
    
    private SprayCanScript sprayCanScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprayCanScript = FindObjectOfType<SprayCanScript>();
        canvasTexture = new Texture2D(renderTexture.width, renderTexture.height);
        Color[] pixels = Enumerable.Repeat(Color.black, renderTexture.width * renderTexture.height).ToArray();
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();
        canvasMaterial.SetTexture("_RenderTexture", canvasTexture);
        stencilTexture = stencil.GetComponent<Renderer>().material.mainTexture as Texture2D;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && sprayCanScript.FollowingMouse)
        {
            stencilTexture = stencil.GetComponent<Renderer>().material.mainTexture as Texture2D;
        }
        if (Input.GetMouseButton(0) && sprayCanScript.FollowingMouse) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit hit;
            RaycastHit[] hits = Physics.RaycastAll(ray, 1, paintLayer);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject == gameObject || hit.collider.gameObject.CompareTag("stencil"))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        canvasHitCoord = hit.textureCoord;
                    }

                    needUpdate = true;
                }
            }
        }
    }
    
    private Vector2 blCoord;
    private Vector2 trCoord;

    private (Vector2, Vector2) GetStencilUvBlTr()
    {
        Transform bl = stencil.transform.Find("BL");
        Transform tr = stencil.transform.Find("TR");

        Vector3 blforward = bl.forward;
        Vector3 trforward = tr.forward;

        Ray blRay = new Ray(bl.transform.position + blforward * 0.01f, blforward);
        Ray trRay = new Ray(tr.transform.position + trforward * 0.01f, trforward);

        if (Physics.Raycast(blRay, out RaycastHit hit, 1, paintLayer))
        {
            blCoord = hit.textureCoord;
        }

        if (Physics.Raycast(trRay, out hit, 1, paintLayer))
        {
            trCoord = hit.textureCoord;
        }
        
        return (blCoord, trCoord);
    }

    private void LateUpdate()
    {
        if (needUpdate)
        {
            var stenciluvs = GetStencilUvBlTr();
            int x = (int)Mathf.Lerp(0, renderTexture.width, canvasHitCoord.x);
            int y = (int)Mathf.Lerp(0, renderTexture.height, canvasHitCoord.y);
            // texture.SetPixel(x, y, new Color(1, 0, 0));
            DrawCircle(canvasTexture, new Color(1-sprayCanScript.SprayColor.r, 1-sprayCanScript.SprayColor.g, 1-sprayCanScript.SprayColor.b), stenciluvs, x, y, radius);
            RenderTexture.active = null;
            needUpdate = false;
            canvasTexture.Apply();
            canvasMaterial.SetTexture("_RenderTexture", canvasTexture);
        }
    }

    public void DrawCircle(Texture2D tex, Color color, (Vector2 bluv, Vector2 truv) stencilUvs, int x, int y, int radius = 10)
    {
        float rSquared = radius * radius;
        
        float tuvw = stencilUvs.truv.x - stencilUvs.bluv.x;
        float tuvh = stencilUvs.truv.y - stencilUvs.bluv.y;

        float xs = ((float)x / renderTexture.width - stencilUvs.bluv.x) / tuvw;
        float ys = ((float)y / renderTexture.height - stencilUvs.bluv.y) / tuvh;
        
        Debug.Log($"M;EOPW x:{xs},  y:{ys}  bluv: {stencilUvs.bluv},  truv: {stencilUvs.truv}");
        
        if (renderTexture.width <= 0 || renderTexture.height <= 0)
        {
            Debug.LogError("Cannot have 0 width or height.");
            return;
        }

        for (int xPix = Math.Max(x - radius, 0); xPix < MathF.Min(x + radius + 1, renderTexture.width); xPix += 1)
        for (int yPix = Math.Max(y - radius, 0); yPix < MathF.Min(y + radius + 1, renderTexture.height); yPix += 1)
            if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) < rSquared)
            {
                // Transform from pixel space to uv
                var uv = new Vector2((float)xPix / renderTexture.width, (float)yPix / renderTexture.height);
                
                // Transform from canvas UV-space to Stencil-UV space
                var stencilUv = new Vector2(
                    ((float)xPix / renderTexture.width - stencilUvs.bluv.x) / tuvw,
                    ((float)yPix / renderTexture.height - stencilUvs.bluv.y) / tuvh
                );
                
                bool anyOver = stencilUv.x < 0 ||
                               stencilUv.x > 1 ||
                               stencilUv.y < 0 ||
                               stencilUv.y > 1;
                
                if (!anyOver && stencilTexture.GetPixelBilinear(stencilUv.x, stencilUv.y) == Color.black)
                {
                    continue;
                }

                tex.SetPixel(xPix, yPix, color);
            }
    }
}
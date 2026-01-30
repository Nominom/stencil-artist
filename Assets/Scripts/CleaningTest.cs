using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CleaningTest : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Material cleanMat;
    MeshRenderer meshRenderer;

    [Range(1, 100)]
    public int radius = 10;

    Texture2D texture;

    private bool needUpdate = false;

    private Vector2 canvasHitCoord;
    public GameObject stencil;
    bool stencilHit = false;
    private Vector2 stencilHitCoord;
    private Texture2D stencilTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texture = new Texture2D(renderTexture.width, renderTexture.height);
        Color[] pixels = Enumerable.Repeat(Color.black, renderTexture.width * renderTexture.height).ToArray();
        texture.SetPixels(pixels);
        texture.Apply();
        cleanMat.SetTexture("_RenderTexture", texture);
        stencilTexture = stencil.GetComponent<Renderer>().material.mainTexture as Texture2D;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit hit;
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject == gameObject || hit.collider.gameObject.CompareTag("stencil"))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        canvasHitCoord = hit.textureCoord;
                    }

                    if (hit.collider.gameObject.CompareTag("stencil"))
                    {
                        stencilHit = true;
                        stencilHitCoord = hit.textureCoord;
                    }
                    else
                    {
                        stencilHit = false;
                    }
                    //     Texture2D tex = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;
                    //     stencilTexture = tex;
                    //     // if (tex.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y) == Color.black)
                    //     // {
                    //     //     Debug.Log("black");
                    //     //     needUpdate = false;
                    //     // }
                    // }

                    // Debug.Log(hit.textureCoord);


                    needUpdate = true;
                    // texture.Apply();
                    // cleanMat.SetTexture("_RenderTexture", texture);
                }
            }
        }
    }

    private (Vector2, Vector2) GetStencilUvBlTr()
    {
        Transform bl = stencil.transform.Find("BL");
        Transform tr = stencil.transform.Find("TR");

        Vector2 blCoord = default;
        Vector2 trCoord = default;

        Vector3 blforward = bl.forward;
        Vector3 trforward = tr.forward;

        Ray blRay = new Ray(bl.transform.position + blforward * 0.01f, blforward);
        Ray trRay = new Ray(tr.transform.position + trforward * 0.01f, trforward);

        if (Physics.Raycast(blRay, out RaycastHit hit, 1))
        {
            blCoord = hit.textureCoord;
        }

        if (Physics.Raycast(trRay, out hit, 1))
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
            DrawCircle(texture, new Color(1, 0, 1), stenciluvs, x, y, radius);
            RenderTexture.active = null;
            needUpdate = false;
            texture.Apply();
            cleanMat.SetTexture("_RenderTexture", texture);
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
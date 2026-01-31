using UnityEngine;

public class StencilScript : MonoBehaviour
{
    public StencilScobj stencil;
    StencilScobj previousStencil;
    Camera cam;

    private bool followMouse = false;

    public bool FollowingMouse
    {
        get => followMouse;
        set => followMouse = value;
    }
    private SprayCanScript sprayCanScript;

    public int currentStencilPixelsPainted = 0;
    public bool stencilUsed = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        sprayCanScript = FindObjectOfType<SprayCanScript>();
        UpdateStencil();
    }


    public void UpdateStencil()
    {
        if (stencil == null)
            return;
        if (stencil == previousStencil)
            return;
        
        previousStencil = stencil;
        transform.localScale = Vector3.one * stencil.size;
        GetComponent<MeshRenderer>().material.mainTexture = stencil.texture;
        stencilUsed = false;
        currentStencilPixelsPainted = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (followMouse)
        {
            Vector3 position = Input.mousePosition;
            position.z = 6;
            position = cam.ScreenToWorldPoint(position);
            transform.position = position;
        }

        if (Input.GetMouseButtonUp(1))
        {
            followMouse = false;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("clicka de stencil");
        if(!sprayCanScript.FollowingMouse)
        {
            followMouse = true;
            currentStencilPixelsPainted = 0;
        }
    }
}

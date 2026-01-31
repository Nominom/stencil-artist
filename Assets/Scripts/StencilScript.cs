using Unity.VisualScripting;
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
    private CanvasPaintingScript canvas;

    public int currentStencilPixelsPainted = 0;
    public bool stencilUsed = false;

    public float defaultScale = 0.3f;

    public float grabZ = 0.9f;
    
    bool firstClick = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        sprayCanScript = FindObjectOfType<SprayCanScript>();
        canvas = FindObjectOfType<CanvasPaintingScript>();
        UpdateStencil();
    }


    public void UpdateStencil()
    {
        if (stencil == null)
            return;
        if (stencil == previousStencil)
            return;

        previousStencil = stencil;
        transform.localScale = Vector3.one * defaultScale * stencil.size;
        GetComponent<MeshRenderer>().material.mainTexture = stencil.texture;
        stencilUsed = false;
        currentStencilPixelsPainted = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (followMouse)
        {
            Plane plane = new Plane(canvas.transform.forward, canvas.transform.position);

            Vector3 position = Input.mousePosition;
            position.z = 1;
            position = cam.ScreenToWorldPoint(position);

            Vector3 planePoint = plane.ClosestPointOnPlane(position);
            planePoint -= canvas.transform.forward * grabZ;
            transform.position = planePoint;
            transform.forward = plane.normal;
        }

        if (Input.GetMouseButtonUp(1))
        {
            followMouse = false;
        }
    }

    private void OnMouseOver()
    {
        FindFirstObjectByType<UIMEOW>().OnStencilSelected(stencil);
    }

    private void OnMouseDown()
    {
        if (sprayCanScript.FollowingMouse)
        {
            return;
        }
        
        if (firstClick)
        {
            FindObjectOfType<StencilSelection>().KillOtherStencils(this);
            firstClick = false;
        }
        Debug.Log("clicka de stencil");
        if (stencilUsed)
        {
            KillStencil();
            FindObjectOfType<StencilSelection>().LoadNewSelection();
        }

        if (!sprayCanScript.FollowingMouse && !stencilUsed)
        {
            followMouse = true;
            currentStencilPixelsPainted = 0;
        }
    }

    public void KillStencil()
    {
        transform.AddComponent<Rigidbody>();
        transform.GetComponent<Rigidbody>().angularVelocity = transform.forward * Random.Range(-5, 5);
        Invoke("Die", 0.5f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
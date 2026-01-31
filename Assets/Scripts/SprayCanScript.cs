using System;
using UnityEngine;

public class SprayCanScript : MonoBehaviour
{
    Material mat;
    Color color;
    public Color SprayColor { get => color; set => color = value; }
    Camera cam;
    public LayerMask layer;
    public ParticleSystem ps;
    public CanvasPaintingScript canvas;
    private bool followMouse = false;
    private StencilScript stencil;

    public bool FollowingMouse
    {
        get => followMouse;
        set => followMouse = value;
    }

    private void Awake()
    {
        mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
        color = mat.color;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
        canvas = FindFirstObjectByType<CanvasPaintingScript>();
        stencil = FindAnyObjectByType<StencilScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (followMouse)
        {
            Vector3 position = Input.mousePosition;
            position.z = 3;
            position = cam.ScreenToWorldPoint(position);
            transform.position = position;
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layer))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.tag == "spraycan")
                {
                    color = hit.collider.GetComponent<MeshRenderer>().material.color;
                    mat.color = color;
                    var col = ps.colorOverLifetime;
                    Gradient gradient = new Gradient();
                    gradient.SetKeys( new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
                    col.color = gradient;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            followMouse = false;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("clicka de spraya");
        if(!stencil.FollowingMouse)
        {
            followMouse = true;
        }
    }
}

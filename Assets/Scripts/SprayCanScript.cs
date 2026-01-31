using System;
using UnityEngine;

public class SprayCanScript : MonoBehaviour
{
    Material mat;
    Color color;
    Camera cam;
    public LayerMask layer;
    public ParticleSystem ps;

    private bool followMouse = false;

    public bool FollowingMouse
    {
        get => followMouse;
        set => followMouse = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
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
        followMouse = true;
    }
}

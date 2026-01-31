using UnityEngine;

public class StencilScript : MonoBehaviour
{
    Camera cam;

    private bool followMouse = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        
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
        followMouse = true;
    }
}

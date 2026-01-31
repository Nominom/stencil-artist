using UnityEngine;

public class SprayCanScript : MonoBehaviour
{
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Input.mousePosition;
        position.z = 3;
        position = cam.ScreenToWorldPoint(position);
        transform.position = position;
    }
}

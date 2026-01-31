using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMeower : MonoBehaviour
{
    [Tooltip("The screen x coordinate from the screen side where we rotate.")]
    public int CameraTurnXMargin = 50;
    public float CameraTurnSpeed = 10f;

    void Update()
    {
        if (Mouse.current.position.value.x < CameraTurnXMargin)
        {
            Camera.main.transform.Rotate(Vector3.up, -CameraTurnSpeed * Time.deltaTime);
        }
        else if (Mouse.current.position.value.x >= Screen.width - CameraTurnXMargin)
        {
            Camera.main.transform.Rotate(Vector3.up, CameraTurnSpeed * Time.deltaTime);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMeower : MonoBehaviour
{
    [Tooltip("The screen x coordinate from the screen side where we rotate.")]
    public int CameraTurnXMargin = 50;
    public float CameraMoveTime = 1f;
    public AnimationCurve CameraMoveCurve;

    public Transform CameraPositionLeft;
    public Transform CameraPositionRight;

    public enum CameraPositions
    {
        Left,
        Right,
    }
    public CameraPositions CameraPosition;
    
    

    void Update()
    {
        if (Mouse.current.position.value.x < CameraTurnXMargin)
        {
            CameraPosition = CameraPositions.Left;
        }
        else if (Mouse.current.position.value.x >= Screen.width - CameraTurnXMargin)
        {
            CameraPosition = CameraPositions.Right;
        }

    }
}

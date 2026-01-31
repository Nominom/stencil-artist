using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMeower : MonoBehaviour
{
    [Tooltip("The screen x coordinate from the screen side where we rotate.")]
    public int CameraTurnXMargin = 50;
    public float CameraMoveTime = 1f;
    public AnimationCurve CameraMoveCurve;

    public Vector3 CameraPositionLeft;
    public Vector3 CameraRotationLeft;
    public Vector3 CameraPositionRight;
    public Vector3 CameraRotationRight;

    public enum CameraPositions
    {
        Left,
        Right,
    }
    public CameraPositions CameraPosition;
    private bool moving = false;
    private float movingTimer = 0f;
    private Vector3 targetPosition;
    private Vector3 targetRotation;

    void Update()
    {
        if (Mouse.current.position.value.x < CameraTurnXMargin)
        {
            if (CameraPosition == CameraPositions.Left)
                return;

            moving = true;
            targetPosition = CameraPositionLeft;
            targetRotation = CameraRotationLeft;
        }
        else if (Mouse.current.position.value.x >= Screen.width - CameraTurnXMargin)
        {
            if (CameraPosition == CameraPositions.Right)
                return;

            moving = true;
            targetPosition = CameraPositionRight;
            targetRotation = CameraRotationRight;
        }

        if (moving)
        {
            if (CameraMoveTime == 0f)
            {
                moving = false;
                Camera.main.transform.position = targetPosition;
                Camera.main.transform.rotation = Quaternion.Euler(targetRotation);
                return;
            }

            movingTimer += Time.deltaTime;
            if (CameraPosition == CameraPositions.Left)
            {
                Camera.main.transform.position = targetPosition;
                Camera.main.transform.Rotate((CameraRotationRight - CameraRotationLeft) * CameraMoveCurve.Evaluate(movingTimer / CameraMoveTime));
            }
            else if (CameraPosition == CameraPositions.Right)
            {
                
                Camera.main.transform.position = targetPosition;
                Camera.main.transform.Rotate((CameraRotationLeft - CameraRotationRight) * CameraMoveCurve.Evaluate(movingTimer / CameraMoveTime));
            }

            if (movingTimer >= CameraMoveTime)
            {
                moving = false;
                movingTimer = 0f;
            }
        }
    }
}

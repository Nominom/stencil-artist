using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMeower : MonoBehaviour
{
    [Tooltip("The screen x coordinate from the screen side where we rotate.")]
    public int CameraTurnXMargin = 50;
    public AnimationCurve CameraMoveCurve;

    public float lerpDuration = 0.5f;
    public Transform cameraPositionLeft;
    public Transform cameraPositionRight;

    public enum CameraPositions
    {
        Left,
        Right,
    }
    public CameraPositions CameraPosition;

    private Coroutine lerpRoutine;
    

    void Update()
    {
        if (Mouse.current.position.value.x < CameraTurnXMargin)
        {
            if (lerpRoutine == null && CameraPosition != CameraPositions.Left)
            {
                CameraPosition = CameraPositions.Left;
                lerpRoutine = StartCoroutine(Lerpa(cameraPositionLeft));
            }
        }
        else if (Mouse.current.position.value.x >= Screen.width - CameraTurnXMargin)
        {
            if (lerpRoutine == null && CameraPosition != CameraPositions.Right)
            {
                CameraPosition = CameraPositions.Right;
                lerpRoutine = StartCoroutine(Lerpa(cameraPositionRight));
            }
        }
    }

    IEnumerator Lerpa(Transform target)
    {
        Debug.Log("Panning to " + target.name, gameObject);
        
        if (target == null)
            yield break;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition = target.position;
        Quaternion endRotation = target.rotation;

        float t = 0;

        while (t < lerpDuration)
        {
            t += Time.deltaTime;
            
            float i = CameraMoveCurve.Evaluate(t / lerpDuration);
            transform.position = Vector3.Lerp(startPosition, endPosition, i);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, i);
            
            yield return null;
        }
        
        lerpRoutine = null;
    }
    
    
}

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Flags]
public enum FlickerMode
{
    None = 0,
    Color = 1 << 0,
    Intensity = 1 << 1,
    Range = 1 << 2
}

[RequireComponent(typeof(Light))]
public class FlameFlicker : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float minRange = 5f;
    public float maxRange = 10f;
    public float flickerSpeed = 0.1f;
    public Gradient colorGradient;

    private Light _light;
    private Coroutine _flickerCoroutine;

    public FlickerMode flickerMode;

    private void OnEnable()
    {
        _light = GetComponent<Light>();
        _flickerCoroutine = StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            if (flickerMode.HasFlag(FlickerMode.Intensity))
            {
                float intensity = Random.Range(minIntensity, maxIntensity);
                _light.intensity = intensity;
            }

            if (flickerMode.HasFlag(FlickerMode.Color))
            {
                _light.color = colorGradient.Evaluate(Random.Range(0f, 1f));
            }

            if (flickerMode.HasFlag(FlickerMode.Range))
            {
                float range = Random.Range(minRange, maxRange);
                _light.range = range;
            }

            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
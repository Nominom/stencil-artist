using System;
using UnityEngine;

public class Meow : MonoBehaviour
{
    public float CatPurrInterval;

    private AudioSource _audioSource;
    private float _catPurrTimer = 0;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _catPurrTimer += Time.deltaTime;
        if (_catPurrTimer >= CatPurrInterval)
        {
            _catPurrTimer = 0f;
            _audioSource.Play();
        }
    }
}

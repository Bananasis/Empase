using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GravitySoundEffect : MonoBehaviour
{
    [SerializeField, Inject, HideInInspector]
    private AudioManager _audioManager;

    [SerializeField] private TrackEnum _track;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = _audioManager.AddAudioSource(gameObject, _track);
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void OnEnable()
    {
        if (_audioSource is null) return;
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _audioSource.Pause();
    }
}
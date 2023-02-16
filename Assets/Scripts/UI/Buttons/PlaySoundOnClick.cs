using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class PlaySoundOnClick : MonoBehaviour
{
    [Inject] private AudioManager _audioManager;
    [SerializeField] private TrackEnum track;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => _audioManager.Play(track));
    }
}
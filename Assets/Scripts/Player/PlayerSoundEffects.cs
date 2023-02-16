using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Zenject;

[RequireComponent(typeof(Player))]
public class PlayerSoundEffects : MonoBehaviour
{
    [Inject] private AudioManager _audioManager;
    [Inject] private LevelManager _levelManager;
    private Player player;
    private float size;
    private float absorbed = 0;
    private float absorb = 0;
    private UnityEvent update = new UnityEvent();
    private AudioSource onAbsorbed;
    private AudioSource onAbsorb;

    private void Start()
    {
        player = GetComponent<Player>();
        var death = _audioManager.AddAudioSource(gameObject, TrackEnum.Death);
        onAbsorb = _audioManager.AddAudioSource(gameObject, TrackEnum.Absorb, () => absorb,
            () => _levelManager.paused != PauseState.Play, update, update);
        onAbsorbed = _audioManager.AddAudioSource(gameObject, TrackEnum.Absorbed, () => absorbed,
            () => _levelManager.paused != PauseState.Play, update, update);

        onAbsorbed.Play();
        onAbsorb.Play();
        onAbsorb.loop = true;
        onAbsorbed.loop = true;
        player.OnConsume.AddListener((_) =>
        {
            death.Stop();
            death.Play();
        });
        player.OnConsumed.AddListener((_) =>
        {
            death.Stop();
            death.Play();
        });

        player.OnSizeChange.Subscribe(tuple =>
        {
            if (size == 0) size = tuple.Item1;
            if (size > tuple.Item1) absorbed = Mathf.Clamp01(absorbed + Time.fixedDeltaTime * 12);
            if (size < tuple.Item1) absorb = Mathf.Clamp01(absorb + Time.fixedDeltaTime * 12);
        });
        player.OnDeath.Subscribe(() => size = 0);
    }

    private void OnEnable()
    {
        onAbsorbed?.Play();
        onAbsorb?.Play();
    }

    private void OnDisable()
    {
        onAbsorbed.Stop();
        onAbsorb.Stop();
    }

    private void FixedUpdate()
    {
        absorb = Mathf.Clamp01(absorb - Time.fixedDeltaTime * 10);
        absorbed = Mathf.Clamp01(absorbed - Time.fixedDeltaTime * 10);
        update.Invoke();
    }


}
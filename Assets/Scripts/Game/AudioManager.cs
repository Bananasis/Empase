using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private TrackHolder _trackHolder;
    [SerializeField] private Transform player;
    [Inject] private GameManager _gameManager;
    private Dictionary<TrackEnum, AudioSource> audioSources = new Dictionary<TrackEnum, AudioSource>();
    private AudioSource CurOstAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var key in _trackHolder.trackDict.Keys)
        {
            switch (_trackHolder.trackDict[key].type)
            {
                case TrackType.Ost:
                {
                    var audioSource = AddAudioSource(gameObject, key);
                    audioSources[key] = audioSource;
                    audioSource.loop = true;
                    break;
                }
                case TrackType.UiEffect:
                {
                    var audioSource = AddAudioSource(gameObject, key);
                    audioSources[key] = audioSource;
                    break;
                }
            }
        }
    }

    public AudioSource AddAudioSource(GameObject go, TrackEnum trackEnum, bool subscribe = true)
    {
        var track = _trackHolder.trackDict[trackEnum];
        var audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = track.clip;
        audioSource.pitch = track.pitch;
        audioSource.volume = track.volume * _gameManager.SoundVolume[track.type] * _gameManager.MasterSoundVolume.val;
        audioSource.mute = !_gameManager.SoundOn[track.type] || !_gameManager.MasterSoundOn.val;
        if (!subscribe) return audioSource;
        {
            var audioSourceCapture = audioSource;
            _gameManager.SoundVolume.GetEvent(track.type)
                .AddListener((v) => audioSourceCapture.volume = v * _gameManager.MasterSoundVolume.val);
            _gameManager.SoundOn.GetEvent(track.type)
                .AddListener((on) => audioSourceCapture.mute = !on || !_gameManager.MasterSoundOn.val);
            _gameManager.MasterSoundVolume.OnChange
                .AddListener((v) => audioSourceCapture.volume = v * _gameManager.SoundVolume[track.type]);
            _gameManager.MasterSoundOn.OnChange
                .AddListener((on) => audioSourceCapture.mute = !on || !_gameManager.SoundOn[track.type]);
        }
        return audioSource;
    }

    public AudioSource Play(TrackEnum trackEnum)
    {
        var track = _trackHolder.trackDict[trackEnum];
        switch (track)
        {
            case {type: TrackType.Ost}:
                CurOstAudioSource?.Stop();
                CurOstAudioSource = audioSources[trackEnum];
                CurOstAudioSource.Play();
                return CurOstAudioSource;
                break;

            case {type: TrackType.UiEffect}:
                audioSources[trackEnum].Play();
                return audioSources[trackEnum];
                ;
                break;
        }

        throw new GameException($"Cant play sound effect {trackEnum}");
    }

    private void Update()
    {
        transform.position = player.position;
    }

    public void Stop(TrackEnum trackEnum)
    {
        var track = _trackHolder.trackDict[trackEnum];
        switch (track)
        {
            case {type: TrackType.Ost}:
                if (CurOstAudioSource == null) return;
                if (CurOstAudioSource.clip != track.clip) return;
                CurOstAudioSource.Stop();
                CurOstAudioSource = null;

                break;
            case {type: TrackType.UiEffect}:
            case {type: TrackType.SoundEffect}:
                throw new GameException($"Cant stop non-OST sound effect {trackEnum}");
                break;
        }
    }
}
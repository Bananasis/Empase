using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Utils;
using Zenject;

public class PlayOstOnOpen : MonoBehaviour
{
    [Inject] private AudioManager _audioManager;
    [SerializeField] private List<TrackEnum> osts;

    [SerializeField] private bool playNext = true;
    private TrackEnum _track;

    private void OnEnable()
    {
        Play();
    }

    private void Play(bool stopPrevious = false)
    {
        if (stopPrevious) Stop();
        _track = osts[Random.Range(0, osts.Count)];
        AudioSource audioSource = _audioManager.Play(_track);
        if (!playNext) return;
        StartCoroutine(PlayNext(audioSource.clip.length));
    }

    private IEnumerator PlayNext(float secWait)
    {
       yield return new WaitForSeconds(secWait);
       Play(true);
    }

    private void Stop()
    {
        _audioManager.Stop(_track);
    }

    private void OnDisable()
    {
        Stop();
    }
}
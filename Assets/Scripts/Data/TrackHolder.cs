using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Track
{
    public AudioClip clip;
    public float pitch;
    public float volume;
    public TrackType type;
    public TrackEnum track;
}

[CreateAssetMenu(fileName = "TrackHolder", menuName = "ScriptableObjects/TrackHolder", order = 1)]
public class TrackHolder : ScriptableObject
{
    [SerializeField] private List<Track> _tracks = new List<Track>();

    public Dictionary<TrackEnum, Track> trackDict
    {
        get
        {
            if (_trackDict == null) Generate();
            return _trackDict;
        }
    }


    private Dictionary<TrackEnum, Track> _trackDict;

    private void Generate()
    {
        _trackDict = new Dictionary<TrackEnum, Track>();
        _tracks.ForEach((t) => _trackDict[t.track] = t);
    }
}

public enum TrackType
{
    Ost,
    UiEffect,
    SoundEffect
}

public enum TrackEnum
{
    UIButton,
    MenuOst,
    UIBack,
    LevelOst
}
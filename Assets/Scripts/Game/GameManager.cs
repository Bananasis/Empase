using System;
using System.Linq;
using UnityEngine;
using Utils;
using Zenject;


public partial class GameManager : MonoBehaviour
{
    public PlayerPrefDictionary<TrackType, bool> SoundOn;
    public PlayerPrefDictionary<TrackType, float> SoundVolume;
    public PlayerPrefBool MasterSoundOn;
    public PlayerPrefFloat MasterSoundVolume;
    public PlayerPrefEnumList<LevelCompletion> Levels;
    public PlayerPrefDictionary<ShiftType, bool> ShiftAbilities;
    public PlayerPrefDictionary<PropulsionType, bool> MouseAbilities;
    public PlayerPrefEnum<PropulsionType> CurMouseAbility;
    public PlayerPrefEnum<ShiftType> CurShiftAbility;
    public PlayerPrefDictionary<Achievement, bool> Achievements;
    public PlayerPrefInt CurrentLevel;
    public PlayerPrefInt LastOpenZone;
    public PlayerPrefInt CurZone;


    public void Awake()
    {
        MasterSoundOn = new PlayerPrefBool("MasterSoundON", true);
        MasterSoundVolume = new PlayerPrefFloat("MasterSoundVolume", 0.4f);
        SoundOn = new PlayerPrefDictionary<TrackType, bool>("SoundOn", true,
            Enum.GetValues(typeof(TrackType)).Cast<TrackType>().ToList());
        SoundVolume = new PlayerPrefDictionary<TrackType, float>("SoundVolume", 0.4f,
            Enum.GetValues(typeof(TrackType)).Cast<TrackType>().ToList());
        Levels = new PlayerPrefEnumList<LevelCompletion>("Levels", LevelCompletion.Closed, 10);
        CurrentLevel = new PlayerPrefInt("CurLevel");
        LastOpenZone = new PlayerPrefInt("OpenZone");
        CurZone = new PlayerPrefInt("CurZone");
        if (Levels[0] == LevelCompletion.Closed)
        {
            Levels[0] = LevelCompletion.Open;
        }

        LastOpenZone.OnChange.AddListener((z) => CurZone.val = z);
        ShiftAbilities =
            new PlayerPrefDictionary<ShiftType, bool>("ShiftAbilities", false,
                Enum.GetValues(typeof(ShiftType)).Cast<ShiftType>().ToList());
        MouseAbilities =
            new PlayerPrefDictionary<PropulsionType, bool>("MouseAbilities", false,
                Enum.GetValues(typeof(PropulsionType)).Cast<PropulsionType>().ToList());
        ShiftAbilities[ShiftType.None] = true;
        MouseAbilities[PropulsionType.Propulsion] = true;
        Achievements = new PlayerPrefDictionary<Achievement, bool>("Achievements", false,
            Enum.GetValues(typeof(Achievement)).Cast<Achievement>().ToList());
        CurMouseAbility = new PlayerPrefEnum<PropulsionType>("MouseAbility", PropulsionType.Propulsion);
        CurShiftAbility = new PlayerPrefEnum<ShiftType>("ShiftAbility", ShiftType.None);
        ShiftAbilities.OnChange.AddListener((sa) =>
        {
            if (sa.Item2) CurShiftAbility.val = sa.Item1;
        });
        MouseAbilities.OnChange.AddListener((sa) =>
        {
            if (sa.Item2) CurMouseAbility.val = sa.Item1;
        });
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
    
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
    }
}

public enum LevelCompletion
{
    Closed,
    Open,
    Passed,
}

public enum Achievement
{
    PassFirstZone,
    ConsumedBySun,
    EventHorizon,
    Consume
}
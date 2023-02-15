using System;
using System.Linq;
using UnityEngine;
using Utils;
using Zenject;


public partial class GameManager : MonoBehaviour
{
    public PlayerPrefBool SoundOn;
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
        SoundOn = new PlayerPrefBool("SoundOn", true);
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
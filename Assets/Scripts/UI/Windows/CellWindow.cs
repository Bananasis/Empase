using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellWindow : AlphaWindow
{
    [SerializeField]private List<AbilityButton> shifts = new List<AbilityButton>();

    [SerializeField] private List<AbilityButton> propulsions = new List<AbilityButton>();

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        shortcuts[KeyCode.Escape] = () => _windowManager.CloseTopWindow();
        for (var i = 0; i < shifts.Count; i++)
        {
            shifts[i].SetUp((ShiftType)i);
        }
        for (var i = 0; i < propulsions.Count; i++)
        {
            propulsions[i].SetUp((PropulsionType)i);
        }

    }
}

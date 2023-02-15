using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private List<Popup> popUps = new List<Popup>();
    private readonly Dictionary<PopupType, Popup> popupDict = new Dictionary<PopupType, Popup>();
    private List<PopupType> _levelPopups = new List<PopupType>() {PopupType.Comment, PopupType.Tip, PopupType.Title};

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var popUp in popUps)
        {
            popupDict[popUp.type] = popUp;
        }
    }

    public void CreatePopup(PopupData popupData)
    {
        popupDict[popupData.type].Enqueue(popupData);
    }

    public void Clear(PopupType type)
    {
        popupDict[type].Clear();
    }

    public void ClearAll()
    {
        foreach (var popUp in popUps)
        {
            popUp.Clear();
        }
    }

    public void Next(PopupType type)
    {
        popupDict[type].Next();
    }

    public void ClearLevelPopups()
    {
        foreach (var levelPopup in _levelPopups)
        {
            popupDict[levelPopup].Clear();
        }
    }
}

[Serializable]
public struct PopupData
{
    public PopupType type;
    public string text;
    public float duration;
    public bool force;
    public bool vol;
    public Sprite sprite;
}

public enum PopupType
{
    Title,
    Tip,
    Comment,
    Unlock,
    Achievement
}
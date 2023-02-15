using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Zenject;

[Serializable]
public class Announcement : IAnnouncement
{
    [SerializeField] public List<PopupData> PopupDatas = new List<PopupData>();


    public void Show(PopupManager popupManager)
    {
        foreach (var popupData in PopupDatas)
        {
            popupManager.CreatePopup(popupData);
        }
    }
}
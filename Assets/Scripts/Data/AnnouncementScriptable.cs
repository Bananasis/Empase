using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Announcement", menuName = "ScriptableObjects/Announcement", order = 1)]
public class AnnouncementScriptable : ScriptableObject, IAnnouncement
{
    private readonly Announcement _announcement = new Announcement();


    public void Show(PopupManager popupManager)
    {
        _announcement.Show(popupManager);
    }
}
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public interface IAnnouncement
{
    void Show(PopupManager popupManager);
}
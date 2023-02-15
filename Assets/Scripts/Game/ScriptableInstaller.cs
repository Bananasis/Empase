using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScriptableInstaller : MonoInstaller
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private CellManager _cellManager;
 
    public override void InstallBindings()
    {
   
    }
       
    
}

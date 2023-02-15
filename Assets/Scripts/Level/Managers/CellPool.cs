using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum CellType
{
    Matter,
    Antimatter,
    AntiInertial,
    AntiInertialMatter,
    Attractor,
    Repulsor,
    BlackHole,
    GoTo
}

public partial class CellPool : MonoBehaviour
{
    private Dictionary<CellType, Queue<Cell>> pool = new Dictionary<CellType, Queue<Cell>>();
    [SerializeField] private List<Cell> prefs;
    private Dictionary<CellType, Cell> prefDict = new Dictionary<CellType, Cell>();
    [SerializeField] public Player player;
    private Dictionary<CellType, Transform> categoryTransforms = new Dictionary<CellType, Transform>();


    private void Awake()
    {
        foreach (var pref in prefs)
        {
            prefDict[pref.cData.type] = pref;
            pool[pref.cData.type] = new Queue<Cell>();
            var category = new GameObject(pref.cData.type.ToString())
            {
                transform =
                {
                    parent = transform
                }
            };
            categoryTransforms[pref.cData.type] = category.transform;
        }
    }

    public Cell CreateCell(CellData cd)
    {
        prefDict[cd.type].cData = cd;
        var cell = Instantiate(prefDict[cd.type], cd.position, Quaternion.identity, categoryTransforms[cd.type]);
        cell.gameObject.SetActive(false);
        return cell;
    }
}
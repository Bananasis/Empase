using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControll : MonoBehaviour,IShiftAbility
{
    public IInputProvider _inputProvider { get; set; }
    public ShiftType type => ShiftType.Attraction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

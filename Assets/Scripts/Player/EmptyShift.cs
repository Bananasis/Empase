using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyShift : MonoBehaviour,IShiftAbility
{ 
    public IInputProvider _inputProvider { get; set; }
    public ShiftType type => ShiftType.None;
}

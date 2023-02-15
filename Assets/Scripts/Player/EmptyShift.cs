using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyShift : MonoBehaviour,IShiftAbility
{ 
    public ShiftType type => ShiftType.None;
}

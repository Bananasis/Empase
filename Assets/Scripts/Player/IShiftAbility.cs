public interface IShiftAbility
{
    IInputProvider _inputProvider { set; }
    ShiftType type { get; }
}
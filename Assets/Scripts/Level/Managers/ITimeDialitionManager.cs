public interface ITimeDialitionManager : IRegistry<TimeDialator>
{
    float GetTime(CellData cellData);
}

public partial class TimeDialitionManager : ITimeDialitionManager
{
    public float GetTime(CellData cellData)
    {
        float dialition = 1;
        foreach (var timeDialator in reg)
        {
            dialition *= timeDialator.GetDialition(cellData);
        }

        return dialition;
    }
}
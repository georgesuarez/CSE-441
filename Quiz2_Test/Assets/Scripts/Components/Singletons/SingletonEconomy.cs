using Unity.Entities;

public struct SingletonEconomy : IComponentData
{
    public float Gold;
    public float Lumber;
    public int Population;
    public bool WinCondition;
}

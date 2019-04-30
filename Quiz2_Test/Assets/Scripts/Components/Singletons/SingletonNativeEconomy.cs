using Unity.Entities;

public struct SingletonNativeEconomy : IComponentData
{
    public float Gold;
    public float Lumber;
    public int Population;
    public bool WinCondition;
}

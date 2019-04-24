using Unity.Entities;

public struct MeteorSpawner : IComponentData
{
    public Entity Prefab;
    public int Damage;
}

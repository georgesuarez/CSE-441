using Unity.Entities;
using Unity.Collections;

public class DestroySystem : ComponentSystem
{
    public EntityQuery entityQuery;

    protected override void OnCreateManager()
    {
        entityQuery = GetEntityQuery(typeof(Destroy));
    }

    protected override void OnUpdate()
    {
        using (var destroys = entityQuery.ToEntityArray(Allocator.TempJob))
        {
            foreach (var destroy in destroys)
            {
                EntityManager.DestroyEntity(destroy);
            }
        }
    }
}
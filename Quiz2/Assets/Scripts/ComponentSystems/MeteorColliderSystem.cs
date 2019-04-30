using Unity.Entities;
using Unity.Collections;

public class MeteorColliderSystem : ComponentSystem
{
    public EntityQuery entityQuery;

    protected override void OnCreateManager()
    {
        entityQuery = GetEntityQuery(typeof(Collided));
    }

    protected override void OnUpdate()
    {
        using (var collided = entityQuery.ToEntityArray(Allocator.TempJob))
        {
            foreach (var destroy in collided)
            {
                EntityManager.DestroyEntity(destroy);
            }
        }
    }
}
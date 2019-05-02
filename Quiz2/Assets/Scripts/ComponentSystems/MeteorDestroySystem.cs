using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(MeteorColliderSystem))]
public class MeteorDestroySystem : ComponentSystem
{
    public EntityQuery entityQuery;

    protected override void OnCreateManager()
    {
        entityQuery = GetEntityQuery(typeof(DestroyTag));
    }

    protected override void OnUpdate()
    {
        using (var meteors = entityQuery.ToEntityArray(Allocator.TempJob))
        {
            foreach (var meteor in meteors)
            {
                EntityManager.DestroyEntity(meteor);
            }
        }
    }
}

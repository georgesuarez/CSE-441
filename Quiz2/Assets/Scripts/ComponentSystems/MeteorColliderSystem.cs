using Unity.Entities;
using Unity.Collections;

public class MeteorColliderSystem : ComponentSystem
{
    public EntityQuery meteorQuery;

    protected override void OnCreateManager()
    {
        meteorQuery = GetEntityQuery(typeof(Collided));
    }

    protected override void OnUpdate()
    {
        using (var meteors = meteorQuery.ToEntityArray(Allocator.TempJob))
        {
            foreach (var meteor in meteors)
            {
                EntityManager.DestroyEntity(meteor);
            }
        }
    }
}
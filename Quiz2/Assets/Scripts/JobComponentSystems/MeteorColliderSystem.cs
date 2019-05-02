using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[UpdateBefore(typeof(MeteorDestroySystem))]
public class MeteorColliderSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    [RequireComponentTag(typeof(Collided))]
    struct MeteorCollisionLocationJob : IJobForEachWithEntity<MeteorTag>
    {
        public NativeArray<float3> collisionLocation;
        public EntityCommandBuffer CommandBuffer;

        public void Execute(Entity entity, int index, ref MeteorTag meteor)
        {
            collisionLocation[0] = meteor.collisionSite;
            CommandBuffer.RemoveComponent<Collided>(entity);
            CommandBuffer.AddComponent(entity, new DestroyTag { });
        }
    }

    struct DamageUnitsJob : IJobForEachWithEntity<Health, Translation>
    {
        [ReadOnly] public NativeArray<float3> collisionLocation;
        public EntityCommandBuffer CommandBuffer;
        public float hitPoints;

        public void Execute(Entity entity, int index, ref Health health, ref Translation position)
        {
            var distance = math.distance(collisionLocation[0], position.Value);

            if (distance <= 60)
            {
                if (health.Value > 0f)
                {
                    health.Value -= hitPoints;
                }

                if (health.Value <= 0f)
                {
                    CommandBuffer.DestroyEntity(entity);
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<float3> m_CollisionLocation = new NativeArray<float3>(1, Allocator.TempJob);

        var jobHandle = inputDeps;

        var collisionLocationJob = new MeteorCollisionLocationJob
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
            collisionLocation = m_CollisionLocation
        }.ScheduleSingle(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(collisionLocationJob);
        collisionLocationJob.Complete();
        
        if (m_CollisionLocation.Length > 0)
        {
            var damageUnitsJob = new DamageUnitsJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
                collisionLocation = m_CollisionLocation,
                hitPoints = 50f
            }.ScheduleSingle(this, inputDeps);

            m_EntityCommandBufferSystem.AddJobHandleForProducer(damageUnitsJob);

            damageUnitsJob.Complete();
        }

        m_CollisionLocation.Dispose();

        return jobHandle;

    }
}
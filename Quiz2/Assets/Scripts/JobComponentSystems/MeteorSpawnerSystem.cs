using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class MeteorSpawnerSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    struct MeteorSpawnJob : IJobForEachWithEntity<MeteorSpawner>
    {
        public EntityCommandBuffer CommandBuffer;
        public float3 MouseRaycastPosition;

        public void Execute(Entity entity, int index, ref MeteorSpawner meteorSpawner)
        {
            var instance = CommandBuffer.Instantiate(meteorSpawner.Prefab);

            CommandBuffer.SetComponent(instance, new Translation { Value = MouseRaycastPosition });

        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var keyboard = GetSingleton<SingletonKeyboardInput>();

        return inputDeps;
    }
}

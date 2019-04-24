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
            CommandBuffer.AddComponent(instance, new Target { Destination = float3.zero });
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var keyboard = GetSingleton<SingletonKeyboardInput>();
        var mouse = GetSingleton<SingletonMouseInput>();

        var jobHandle = inputDeps;

        if (keyboard.E_Key)
        {
            keyboard.E_Key = false;
            var meteorSpawnJob = new MeteorSpawnJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
                MouseRaycastPosition = mouse.CurrentMouseRaycastPosition,
            };

            jobHandle = meteorSpawnJob.ScheduleSingle(this, inputDeps);

            jobHandle.Complete();
        }

        return jobHandle;
    }
}

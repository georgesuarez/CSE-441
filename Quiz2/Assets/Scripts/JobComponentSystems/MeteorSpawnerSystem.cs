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

    struct MeteorSpawnJob : IJobForEachWithEntity<MeteorSpawner, Translation>
    {
        public EntityCommandBuffer CommandBuffer;
        public float3 InitialMouseRaycastPosition;

        public void Execute(Entity entity, int index, ref MeteorSpawner meteorSpawner, ref Translation position)
        {
            var instance = CommandBuffer.Instantiate(meteorSpawner.Prefab);

            CommandBuffer.SetComponent(instance, new Translation { Value = position.Value });
            CommandBuffer.AddComponent(instance, new SpellTag { });
            CommandBuffer.AddComponent(instance, new Target { Destination = InitialMouseRaycastPosition });
            CommandBuffer.AddComponent(instance, new MovementSpeed { Value = 400f });
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var keyboard = GetSingleton<SingletonKeyboardInput>();
        var mouse = GetSingleton<SingletonMouseInput>();

        var jobHandle = inputDeps;

        if (keyboard.E_KeyActive)
        {
            if (mouse.LeftClickDown)
            {
                keyboard.E_KeyActive = false;

                SetSingleton<SingletonKeyboardInput>(keyboard);

                var meteorSpawnJob = new MeteorSpawnJob
                {
                    CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
                    InitialMouseRaycastPosition = mouse.InitialMouseRaycastPosition
                };

                jobHandle = meteorSpawnJob.ScheduleSingle(this, inputDeps);

                jobHandle.Complete();
            }
        }


        return jobHandle;
    }
}

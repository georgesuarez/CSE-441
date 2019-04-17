using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using System.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class UnitSpawnerSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    struct UnitSpawnerJob : IJobForEachWithEntity<UnitSpawner, SingletonMouseInput, LocalToWorld>
    {
        public EntityCommandBuffer CommandBuffer;

        public void Execute(Entity entity, int index, ref UnitSpawner spawner, ref SingletonMouseInput mouseInput, ref LocalToWorld location)
        {
            if (mouseInput.LeftClickDown)
            {
                var instance = CommandBuffer.Instantiate(spawner.Prefab);
                var position = mouseInput.MouseRaycastPosition;
                CommandBuffer.SetComponent(instance, new Translation { Value = position });
            }
            //CommandBuffer.DestroyEntity(entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new UnitSpawnerJob
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
        }.ScheduleSingle(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

        return job;
    }
}

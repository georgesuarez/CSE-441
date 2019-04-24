using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

public class UnitHealingSystem : JobComponentSystem
{
    BeginSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    struct UnitHealingJob : IJobForEachWithEntity<Health>
    {
        public EntityCommandBuffer CommandBuffer;
        public float3 MouseRaycastPosition;
        public int healingPoints;

        public void Execute(Entity entity, int index, ref Health health)
        {
            health.Value += healingPoints;
            CommandBuffer.AddComponent(entity, new Translation { Value = MouseRaycastPosition });
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = inputDeps;

        var keyboard = GetSingleton<SingletonKeyboardInput>();
        var mouse = GetSingleton<SingletonMouseInput>();

        if (keyboard.W_Key)
        {
            keyboard.W_Key = false;

            var healingJob = new UnitHealingJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
                MouseRaycastPosition = mouse.CurrentMouseRaycastPosition,
                healingPoints = 5
            };

            jobHandle = healingJob.ScheduleSingle(this, inputDeps);

            jobHandle.Complete();
        }


        return jobHandle;
    }
}

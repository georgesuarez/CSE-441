using Unity.Transforms;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;

public class HealthSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    struct HealthJob : IJobForEachWithEntity<Health>
    {
        public EntityCommandBuffer CommandBuffer;
        public bool rightClick;

        public void Execute(Entity entity, int index, ref Health health)
        {
            if (rightClick)
            {
                health.Value -= 10;
            }

            if (health.Value <= 0)
            {
                var destroy = new Destroy();
                CommandBuffer.AddComponent(entity, destroy);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new HealthJob
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
            rightClick = GetSingleton<SingletonMouseInput>().RightClickDown

        }.ScheduleSingle(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

        return job;
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class MeteaorMovementSystem : JobComponentSystem
{
    BeginSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    [RequireComponentTag(typeof(SpellTag))]
    struct MeteaorMovementJob : IJobForEachWithEntity<MovementSpeed, Translation>
    {
        public EntityCommandBuffer CommandBuffer;
        [ReadOnly] public float deltaTime;
        public float3 MouseRaycastPosition;

        public void Execute(Entity entity, int index, ref MovementSpeed movementSpeed, ref Translation position)
        {
            var distance = math.distance(MouseRaycastPosition, position.Value);
            var direction = math.normalize(MouseRaycastPosition - position.Value);

            if (!(distance < 1))
            {
                position.Value += direction * movementSpeed.Value * deltaTime;
            }
            else if (distance < 1)
            {
                CommandBuffer.AddComponent(entity, new Collided { });
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var mouse = GetSingleton<SingletonMouseInput>();

        var meteaorMovementJob = new MeteaorMovementJob
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
            deltaTime = Time.deltaTime,
            MouseRaycastPosition = mouse.CurrentMouseRaycastPosition
        }.ScheduleSingle(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(meteaorMovementJob);

        return meteaorMovementJob;
    }
}

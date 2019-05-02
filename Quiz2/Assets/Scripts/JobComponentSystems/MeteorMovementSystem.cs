using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class MeteorMovementSystem : JobComponentSystem
{
    BeginSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    [RequireComponentTag(typeof(MeteorSpell))]
    struct MeteorMovementJob : IJobForEachWithEntity<MovementSpeed, Target, Translation, MeteorTag>
    {
        public EntityCommandBuffer CommandBuffer;
        [ReadOnly] public float deltaTime;

        public void Execute(Entity entity, int index, [ReadOnly] ref MovementSpeed movementSpeed, ref Target target, ref Translation position, ref MeteorTag meteor)
        {
            var distance = math.distance(target.Destination, position.Value);
            var direction = math.normalize(target.Destination - position.Value);

            if (!(distance < 1))
            {
                position.Value += direction * movementSpeed.Value * deltaTime;
            }

            if (distance <= 10)
            {
                meteor.collisionSite = position.Value;
                CommandBuffer.AddComponent(entity, new Collided { });
                CommandBuffer.RemoveComponent<MeteorSpell>(entity);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var meteorMovementJob = new MeteorMovementJob
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(),
            deltaTime = Time.deltaTime,
        }.ScheduleSingle(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(meteorMovementJob);

        return meteorMovementJob;
    }
}

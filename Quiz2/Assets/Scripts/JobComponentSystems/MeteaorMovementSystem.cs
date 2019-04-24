using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class MeteaorMovementSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(SpellTag))]
    struct MeteaorMovementJob : IJobForEach<MovementSpeed, Translation>
    {
        [ReadOnly] public float deltaTime;
        public float3 MouseRaycastPosition;

        public void Execute(ref MovementSpeed movementSpeed, ref Translation position)
        {
            var distance = math.distance(MouseRaycastPosition, position.Value);
            var direction = math.normalize(MouseRaycastPosition - position.Value);

            if (!(distance < 1))
            {
                position.Value += direction * movementSpeed.Value * deltaTime;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var mouse = GetSingleton<SingletonMouseInput>();

        var meteaorMovementJob = new MeteaorMovementJob
        {
            deltaTime = Time.deltaTime,
            MouseRaycastPosition = mouse.CurrentMouseRaycastPosition
        };


        return meteaorMovementJob.Schedule(this, inputDeps);
    }
}

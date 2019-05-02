using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Jobs;
using Unity.Entities;

public class IncreaseSpeedSpellSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(TagOrc))]
    struct IncreaseUnitSpeedJob : IJobForEach<MovementSpeed, Translation>
    {
        public float increasedMovementSpeed;
        public float3 CurrentMouseRaycastPosiiton;

        public void Execute(ref MovementSpeed movementSpeed, ref Translation position)
        {
            position.Value = CurrentMouseRaycastPosiiton;
            movementSpeed.Value += increasedMovementSpeed;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = inputDeps;

        var keyboard = GetSingleton<SingletonKeyboardInput>();
        var mouse = GetSingleton<SingletonMouseInput>();

        if (keyboard.R_Key)
        {
            keyboard.R_Key = false;

            var increaseUnitSpeed = new IncreaseUnitSpeedJob
            {
                CurrentMouseRaycastPosiiton = mouse.CurrentMouseRaycastPosition,
                increasedMovementSpeed = 100f
            };

            jobHandle = increaseUnitSpeed.ScheduleSingle(this, inputDeps);

            jobHandle.Complete();
        }
        return jobHandle;
    }
}

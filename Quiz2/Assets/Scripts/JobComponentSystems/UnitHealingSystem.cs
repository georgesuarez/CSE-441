using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

public class UnitHealingSystem : JobComponentSystem
{
    [RequireComponentTag(typeof(TagSelected))]
    struct UnitHealingJob : IJobForEach<Health, Translation>
    {
        public float3 CurrentMouseRaycastPosition;
        public int healingPoints;

        public void Execute(ref Health health, ref Translation position)
        {
            position.Value = CurrentMouseRaycastPosition;

            if (health.Value < 100)
            {
                health.Value += healingPoints;
            }
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
                CurrentMouseRaycastPosition = mouse.CurrentMouseRaycastPosition,
                healingPoints = 5
            };

            jobHandle = healingJob.ScheduleSingle(this, inputDeps);

            jobHandle.Complete();
        }
        return jobHandle;
    }
}

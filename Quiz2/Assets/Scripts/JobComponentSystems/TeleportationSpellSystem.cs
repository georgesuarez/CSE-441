using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class TeleportationSpellSystem : JobComponentSystem
{
    [RequireComponentTag(typeof(TagSelected))]
    struct TeleportationSpellJob : IJobForEachWithEntity<Translation>
    {
        public float3 CurrentMouseRaycastPosition;

        public void Execute(Entity entity, int index, ref Translation position)
        {
            position.Value = CurrentMouseRaycastPosition;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = inputDeps;
        var mouse = GetSingleton<SingletonMouseInput>();
        var keyboard = GetSingleton<SingletonKeyboardInput>();

        if (keyboard.E_Key)
        {
            keyboard.E_Key = false;

            var teleportJob = new TeleportationSpellJob
            {
                CurrentMouseRaycastPosition = mouse.CurrentMouseRaycastPosition,
            };

            jobHandle = teleportJob.ScheduleSingle(this, inputDeps);

            jobHandle.Complete();
        }

        return jobHandle;
    }
}

using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public class EconomySystem : JobComponentSystem
{
    [BurstCompile]
    struct AdditionJob : IJobForEach<Worker>
    {
        public float DeltaTime;
        public SingletonEconomy Resources;

        public void Execute([ReadOnly] ref Worker work)
        {
            Resources.Gold += work.Gold * DeltaTime;
            Resources.Lumber += work.Lumber * DeltaTime;
            Resources.Population += work.Population;

            if ( Resources.Gold + Resources.Lumber >= 10000 )
            {
                Resources.WinCondition = true;
            }
            else
            {
                Resources.WinCondition = false;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new AdditionJob
        {
            DeltaTime = Time.deltaTime,
            Resources = GetSingleton<SingletonEconomy>()
        };

        return job.Schedule(this, inputDeps);
    }
}

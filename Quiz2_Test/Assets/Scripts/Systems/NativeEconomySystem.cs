using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections; // Namespace required for Native Containers
using UnityEngine;

[UpdateAfter(typeof(EconomySystem))]
public class NativeEconomySystem : JobComponentSystem
{
    [BurstCompile]
    struct AdditionJob : IJobForEach<Worker>
    {
        [ReadOnly] public float DeltaTime;
        [ReadOnly] public SingletonNativeEconomy Resources;
        public NativeArray<float> NativeGold;
        public NativeArray<float> NativeLumber;
        public NativeArray<int> NativePopulation;
        public NativeArray<bool> NativeWinCondition;

        public void Execute([ReadOnly] ref Worker work)
        {
            NativeGold[0] += work.Gold * DeltaTime;
            NativeLumber[0] += work.Lumber * DeltaTime;
            NativePopulation[0] += work.Population;

            if (Resources.Gold + Resources.Lumber >= 10000)
            {
                NativeWinCondition[0] = true;
            }
            else
            {
                NativeWinCondition[0] = false;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<float> Gold = new NativeArray<float>(1, Allocator.TempJob);
        NativeArray<float> Lumber = new NativeArray<float>(1, Allocator.TempJob);
        NativeArray<int> Population = new NativeArray<int>(1, Allocator.TempJob);
        NativeArray<bool> WinCondition = new NativeArray<bool>(1, Allocator.TempJob);

        var PlayerEconomy = GetSingleton<SingletonNativeEconomy>();

        var job = new AdditionJob
        {
            DeltaTime = Time.deltaTime,
            Resources = PlayerEconomy,
            NativeGold = Gold,
            NativeLumber = Lumber,
            NativePopulation = Population,
            NativeWinCondition = WinCondition,
        };

        job.Schedule(this, inputDeps).Complete();

        PlayerEconomy.Gold += Gold[0];
        PlayerEconomy.Lumber += Lumber[0];
        PlayerEconomy.Population += Population[0];
        PlayerEconomy.WinCondition = WinCondition[0];

        Gold.Dispose();
        Lumber.Dispose();
        Population.Dispose();
        WinCondition.Dispose();

        SetSingleton<SingletonNativeEconomy>(PlayerEconomy);

        return inputDeps;
    }
}

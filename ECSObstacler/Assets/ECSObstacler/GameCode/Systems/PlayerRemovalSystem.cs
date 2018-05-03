using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms2D;

public class RemovePlayerBarrier : BarrierSystem { }

[UpdateAfter(typeof(UISystem))]
[UpdateAfter(typeof(CollisionSystem))]
public class PlayerRemovalSystem : JobComponentSystem
{
    [Inject] private RemovePlayerBarrier barrier;
    [Inject] private PlayerHealthData data;

    [ComputeJobOptimization]
    struct Job : IJob
    {
        public EntityCommandBuffer Commands;
        [ReadOnly] public ComponentDataArray<Health> health;
        [ReadOnly] public EntityArray Entities;

        public void Execute()
        {
            int length = health.Length;
            for (int i = 0; i < length; i++)
            {
                if (health[i].Value <= 0)
                {
                    Commands.DestroyEntity(Entities[i]);
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new Job() {
            Commands = barrier.CreateCommandBuffer(),
            Entities = data.entity,
            health = data.health
        };
        return job.Schedule(inputDeps);
    }
}

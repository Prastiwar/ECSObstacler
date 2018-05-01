using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms2D;

public class ObstacleRemoveBarrier : BarrierSystem { }

public class ObstaclesRemovalSystem : JobComponentSystem
{
    [Inject] private ObstacleRemoveBarrier barrier;
    [Inject] private RemoveObstacleData data;

    [ComputeJobOptimization]
    struct Job : IJob
    {
        public EntityCommandBuffer Commands;
        [ReadOnly] public EntityArray Entities;
        [ReadOnly] public ComponentDataArray<ObstacleMarker> ObstacleMarker;
        [ReadOnly] public ComponentDataArray<Position2D> Positions;
        [ReadOnly] public Unity.Mathematics.float4 ScreenBorder;

        public void Execute()
        {
            int length = Entities.Length;
            for (int i = 0; i < length; i++)
            {
                if (Utils.IsOnBorder(ScreenBorder, Positions[i].Value) || ObstacleMarker[i].MarkDead)
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
            Positions = data.position,
            ObstacleMarker = data.marker,
            ScreenBorder = ECSObstaclerBootstrap.ScreenBorder
};
        return job.Schedule(inputDeps);
    }
}

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms2D;

public class ObstacleRemoveBarrier : BarrierSystem { }

[UpdateAfter(typeof(CollisionSystem))]
public class ObstaclesRemovalSystem : JobComponentSystem
{
    [Inject] private ObstacleRemoveBarrier barrier;
    [Inject] private RemoveObstacleData data;
    [Inject] private PlayerScoreData playerScore;

    [ComputeJobOptimization]
    struct Job : IJob
    {
        public EntityCommandBuffer Commands;
        public ComponentDataArray<ScoreHolder> ScoreHolder;
        [ReadOnly] public ComponentDataArray<ScoreGiver> ScoreGiver;
        [ReadOnly] public ComponentDataArray<ObstacleMarker> ObstacleMarker;
        [ReadOnly] public ComponentDataArray<Position2D> Positions;
        [ReadOnly] public EntityArray Entities;
        [ReadOnly] public Unity.Mathematics.float4 ScreenBorder;

        public void Execute()
        {
            int length = ObstacleMarker.Length;
            for (int i = 0; i < length; i++)
            {
                if (Utils.IsOnBorder(ScreenBorder, Positions[i].Value))
                {
                    Commands.DestroyEntity(Entities[i]);
                    AddScore(i);
                }
                else if (ObstacleMarker[i].MarkDead)
                {
                    Commands.DestroyEntity(Entities[i]);
                }
            }
        }

        private void AddScore(int i)
        {
            for (int hIndex = 0; hIndex < ScoreHolder.Length; hIndex++)
            {
                var holder = ScoreHolder[hIndex];
                holder.Value += ScoreGiver[i].Value;
                ScoreHolder[hIndex] = holder;
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
            ScreenBorder = ECSObstaclerBootstrap.ScreenBorder,
            ScoreGiver = data.scoreGiver,
            ScoreHolder = playerScore.scoreHolder
        };
        return job.Schedule(inputDeps);
    }
}

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms2D;
using UnityEngine;

public class ObstaclesMoveSystem : JobComponentSystem
{
    [Inject] private ObstacleMoveData data;

    [ComputeJobOptimization]
    struct MoveJob : IJobParallelFor
    {
        [ReadOnly] public float dt;
        public ObstacleMoveData data;

        public void Execute(int index)
        {
            Position2D pos = data.position[index];
            pos.Value += dt * data.speed[index].Value * data.head[index].Value;
            data.position[index] = pos;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job =  new MoveJob {
            data = data,
            dt = Time.deltaTime
        };
        return job.Schedule(data.Length, 32, inputDeps);
    }
}

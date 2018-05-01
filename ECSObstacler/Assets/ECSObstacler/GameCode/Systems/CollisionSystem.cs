using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms2D;
using UnityEngine;

public class CollisionSystem : JobComponentSystem
{
    [Inject] private PlayerData player;
    [Inject] private ObstacleData obstacle;

    private struct Job : IJobParallelFor
    {
        public ComponentDataArray<Health> PlayerHealth;
        public ComponentDataArray<ObstacleMarker> Obstacle;
        [ReadOnly] public ComponentDataArray<Position2D> ObstaclePositions;
        [ReadOnly] public ComponentDataArray<Position2D> PlayerPositions;

        public void Execute(int oIndex)
        {
            int length = PlayerPositions.Length;
            for (int pIndex = 0; pIndex < length; pIndex++)
            {
                var distance = math.distance(PlayerPositions[pIndex].Value, ObstaclePositions[oIndex].Value);
                if (distance <= 1)
                {
                    var health = PlayerHealth[pIndex];
                    var obstacle = Obstacle[oIndex];

                    health.Value--;
                    obstacle.MarkDead = true;

                    PlayerHealth[pIndex] = health;
                    Obstacle[oIndex] = obstacle;
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new Job() {
            ObstaclePositions = obstacle.position,
            Obstacle = obstacle.marker,
            PlayerPositions = player.position,
            PlayerHealth = player.health
        };
        return job.Schedule(obstacle.Length, 8, inputDeps);
    }
}

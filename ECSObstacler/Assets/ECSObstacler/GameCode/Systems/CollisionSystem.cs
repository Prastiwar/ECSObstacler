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

        public void Execute(int index)
        {
            int length = PlayerPositions.Length;
            for (int i = 0; i < length; i++)
            {
                var distance = math.distance(PlayerPositions[i].Value, ObstaclePositions[index].Value);
                if (distance <= 1)
                {
                    var health = PlayerHealth[i];
                    var obstacle = Obstacle[i];

                    health.Value--;
                    obstacle.MarkDead = true;

                    PlayerHealth[i] = health;
                    Obstacle[i] = obstacle;
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

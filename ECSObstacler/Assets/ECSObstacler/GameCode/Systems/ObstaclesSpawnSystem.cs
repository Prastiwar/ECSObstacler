using Unity.Entities;
using UnityEngine;

public class ObstaclesSpawnSystem : ComponentSystem
{
    private float timer;
    [Inject] private PlayerPosData playerPosData;

    protected override void OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= ECSObstaclerBootstrap.GameSettings.SpawnCooldown)
        {
            timer = 0;
            ECSObstaclerBootstrap.CreateObstacle(EntityManager, playerPosData.position[0].Value);
        }
    }
}

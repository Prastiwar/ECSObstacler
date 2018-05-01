using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms2D;
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
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        var puc = PostUpdateCommands;
        var randPos = Utils.GetRandomPosition(ECSObstaclerBootstrap.ScreenBorder);

        puc.CreateEntity(ECSObstaclerBootstrap.ObstacleArchetype);
        puc.SetComponent(new Position2D { Value = randPos });
        puc.SetComponent(new Heading2D { Value = Utils.HeadToPlayer(playerPosData.position[0].Value, randPos) });
        puc.SetComponent(default(ObstacleMarker));
        puc.SetComponent(new MoveSpeed { Value = ECSObstaclerBootstrap.GameSettings.ObstacleSpeed });
        puc.AddSharedComponent(ECSObstaclerBootstrap.ObstacleRenderer);
    }
}

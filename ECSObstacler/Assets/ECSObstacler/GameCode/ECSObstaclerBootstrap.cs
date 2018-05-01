using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

public class ECSObstaclerBootstrap : MonoBehaviour
{
    public static int Score { get; set; }
    public static float4 ScreenBorder { get; private set; }

    public static GameSettings GameSettings { get; private set; }
    public static EntityArchetype PlayerArchetype { get; private set; }
    public static EntityArchetype ObstacleArchetype { get; private set; }
    public static EntityArchetype FloorArchetype { get; private set; }

    public static MeshInstanceRenderer PlayerRenderer { get; private set; }
    public static MeshInstanceRenderer ObstacleRenderer { get; private set; }
    public static MeshInstanceRenderer FloorRenderer { get; private set; }

    public static void NewGame()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        CreatePlayer(entityManager);
        //CreateFloor(entityManager);
        CreateObstacles(entityManager);
        // TODO: create scene
    }

    private static void CreateObstacles(EntityManager entityManager)
    {
        var obstacle = entityManager.CreateEntity(ObstacleArchetype);
        var randPos = Utils.GetRandomPosition(ScreenBorder);
        entityManager.SetComponentData(obstacle, new Position2D { Value = randPos });
        entityManager.SetComponentData(obstacle, new Heading2D { Value = Utils.GetRandomHeading(randPos) });
        entityManager.SetComponentData(obstacle, new MoveSpeed { Value = GameSettings.ObstacleSpeed });
        entityManager.SetComponentData(obstacle, default(ObstacleMarker));
        entityManager.SetComponentData(obstacle, new ScoreGiver { Value = UnityEngine.Random.Range(1, 3) });
        entityManager.AddSharedComponentData(obstacle, ObstacleRenderer);
    }

    private static void CreateFloor(EntityManager entityManager)
    {
        var floor = entityManager.CreateEntity(FloorArchetype);
        // unity didn't implemented it yet..
        //entityManager.SetComponentData(floor, new TransformMatrix {
        //    Value = new float4x4(new float4(Screen.width, 0, 0, 0),
        //                         new float4(0, 5, 0, 0),
        //                         new float4(0, 0, 5, 0),
        //                         new float4(0, 0, 0, 1))
        //});
        entityManager.SetComponentData(floor, new Position2D { Value = new float2(0.0f, -4.5f) });
        entityManager.AddSharedComponentData(floor, FloorRenderer);
    }

    private static void CreatePlayer(EntityManager entityManager)
    {
        var player = entityManager.CreateEntity(PlayerArchetype);
        entityManager.SetComponentData(player, new Position2D { Value = new float2(0.0f, 0.0f) });
        entityManager.SetComponentData(player, new Heading2D { Value = new float2(0.0f, 1.0f) });
        entityManager.SetComponentData(player, new Health { Value = GameSettings.StartHealth });
        entityManager.SetComponentData(player, new MoveSpeed { Value = GameSettings.PlayerSpeed });
        entityManager.SetComponentData(player, new PlayerInput() { Gravity = 5 });
        entityManager.SetComponentData(player, new ScoreHolder() { Value = 0 });
        entityManager.AddSharedComponentData(player, PlayerRenderer);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeScene()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        SetArchetypes(entityManager);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeAfterScene()
    {
        GameSettings = FindObjectOfType<GameSettings>()?.GetComponent<GameSettings>();
        if (!GameSettings)
            return;

        ScreenBorder = Utils.GetScreenBorder();
        PlayerRenderer = Utils.GetLookFromPrototype("PlayerPrototype");
        ObstacleRenderer = Utils.GetLookFromPrototype("ObstaclePrototype");
        FloorRenderer = Utils.GetLookFromPrototype("FloorPrototype");

        World.Active.GetOrCreateManager<UISystem>().InitializeUI(GameSettings.HealthText, GameSettings.ScoreText);
        NewGame(); // TODO: newGame from button
    }

    private static void SetArchetypes(EntityManager entityManager)
    {
        var playerInput = ComponentType.Create<PlayerInput>();
        var playerTag = ComponentType.Create<PlayerMarker>();
        var obstacleTag = ComponentType.Create<ObstacleMarker>();
        var health = ComponentType.Create<Health>();
        var heading2D = ComponentType.Create<Heading2D>();
        var moveSpeed = ComponentType.Create<MoveSpeed>();
        var position2D = ComponentType.Create<Position2D>();
        var transformMatrix = ComponentType.Create<TransformMatrix>();
        var scoreHolder = ComponentType.Create<ScoreHolder>();
        var scoreGiver = ComponentType.Create<ScoreGiver>();

        PlayerArchetype = entityManager.CreateArchetype(
            position2D, transformMatrix, heading2D, moveSpeed, playerTag, scoreHolder, health, playerInput
            );

        ObstacleArchetype = entityManager.CreateArchetype(
            position2D, transformMatrix, heading2D, moveSpeed, obstacleTag, scoreGiver
            );

        FloorArchetype = entityManager.CreateArchetype(
            position2D, transformMatrix
            );
    }

}

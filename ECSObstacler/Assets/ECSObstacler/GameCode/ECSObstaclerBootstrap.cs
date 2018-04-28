using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

public class ECSObstaclerBootstrap : MonoBehaviour
{
    public static GameSettings GameSettings { get; private set; }
    public static EntityArchetype PlayerArchetype { get; private set; }
    public static EntityArchetype ObstacleArchetype { get; private set; }
    public static EntityArchetype FloorArchetype { get; private set; }

    private static MeshInstanceRenderer playerRenderer;
    private static MeshInstanceRenderer obstacleRenderer;
    private static MeshInstanceRenderer floorRenderer;
    
    public static void NewGame()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        CreatePlayer(entityManager);
        //CreateFloor(entityManager);
        // TODO: create scene
    }

    private static void CreateFloor(EntityManager entityManager)
    {
        var floor = entityManager.CreateEntity(ObstacleArchetype);
        //float scale = Screen.width;
        //entityManager.SetComponentData(floor, new TransformMatrix { Value = new float4x4(new float4(scale, 0, 0, 0),
        //                                                                                 new float4(0, scale, 0, 0),
        //                                                                                 new float4(0, 0, scale, 0),
        //                                                                                 new float4(0, 0, 0, 1))
        //});
        entityManager.SetComponentData(floor, new Position2D { Value = new float2(0.0f, -4.5f) });
        entityManager.AddSharedComponentData(floor, floorRenderer);
    }

    private static void CreatePlayer(EntityManager entityManager)
    {
        var player = entityManager.CreateEntity(PlayerArchetype);
        entityManager.SetComponentData(player, new Position2D { Value = new float2(0.0f, 0.0f) });
        entityManager.SetComponentData(player, new Heading2D { Value = new float2(0.0f, 1.0f) });
        entityManager.SetComponentData(player, new Health { Value = GameSettings.PlayerStartHealth });
        entityManager.SetComponentData(player, new MoveSpeed { Value = GameSettings.PlayerSpeed });
        entityManager.SetComponentData(player, new PlayerInput { Move = new float2(0, 0), IsGrounded = new bool1(false) });
        entityManager.AddSharedComponentData(player, playerRenderer);
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

        playerRenderer = Utils.GetLookFromPrototype("PlayerPrototype");
        obstacleRenderer = Utils.GetLookFromPrototype("ObstaclePrototype");
        floorRenderer = Utils.GetLookFromPrototype("FloorPrototype");
        NewGame();
    }

    private static void SetArchetypes(EntityManager entityManager)
    {
        var playerInput = ComponentType.Create<PlayerInput>();
        var health = ComponentType.Create<Health>();
        var heading2D = ComponentType.Create<Heading2D>();
        var moveSpeed = ComponentType.Create<MoveSpeed>();
        var position2D = ComponentType.Create<Position2D>();
        var transformMatrix = ComponentType.Create<TransformMatrix>();

        PlayerArchetype = entityManager.CreateArchetype(
            position2D, transformMatrix, heading2D, moveSpeed, health, playerInput
            );

        ObstacleArchetype = entityManager.CreateArchetype(
            position2D, transformMatrix, heading2D, moveSpeed
            );

        FloorArchetype = entityManager.CreateArchetype(
            position2D, transformMatrix
            );
    }

}

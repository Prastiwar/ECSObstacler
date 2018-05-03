using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;
using System.Threading.Tasks;

public class ECSObstaclerBootstrap : MonoBehaviour
{
    public static float4 ScreenBorder { get; private set; }

    public static GameSettings GameSettings { get; private set; }
    public static EntityArchetype PlayerArchetype { get; private set; }

    public static EntityArchetype ObstacleArchetype { get; private set; }
    public static EntityArchetype FloorArchetype { get; private set; }

    public static MeshInstanceRenderer PlayerRenderer { get; private set; }
    public static MeshInstanceRenderer ObstacleRenderer { get; private set; }
    public static MeshInstanceRenderer FloorRenderer { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeScene()
    {
        World.Active.SetBehavioursActive(false);
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

        SetupUI();
    }

    public static void NewGame()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();

        CreatePlayer(entityManager);
        //CreateFloor(entityManager);
        World.Active.SetBehavioursActive(true);
        Time.timeScale = 1;

        GameSettings.MenuCanvas.gameObject.SetActive(false);
        GameSettings.HUDCanvas.gameObject.SetActive(true);
    }

    public static void GameOver()
    {
        World.Active.SetBehavioursActive(false);
        GameSettings.GameOverCanvas.SetActive(true);
        GameSettings.HUDCanvas.SetActive(false);
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

    public static void CreateObstacle(EntityManager entityManager, float2 playerPos)
    {
        var obstacle = entityManager.CreateEntity(ObstacleArchetype);
        var randPos = Utils.GetRandomPosition(ScreenBorder);
        entityManager.SetComponentData(obstacle, new Position2D { Value = randPos });
        entityManager.SetComponentData(obstacle, new Heading2D { Value = Utils.HeadToPlayer(playerPos, randPos) });
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

    private static void SetupUI()
    {
        GameSettings.PauseButton.onClick.AddListener(TogglePause);
        Parallel.ForEach(GameSettings.MenuButtons, btn => btn.onClick.AddListener(RestartWorld));
        Parallel.ForEach(GameSettings.QuitButtons, btn => btn.onClick.AddListener(ExitGame));
        GameSettings.PlayButton.onClick.AddListener(NewGame);

        ActiveMenu();
    }

    private static void TogglePause()
    {
        Time.timeScale = Time.timeScale < 1 ? 1 : 0;
        GameSettings.PauseCanvas.SetActive(!GameSettings.PauseCanvas.activeSelf);
    }

    private static void RestartWorld()
    {
        World.Active.SetBehavioursActive(false);
        World.Active.GetExistingManager<EntityManager>().DestroyAllEntities();
        ActiveMenu();
    }

    private static void ActiveMenu()
    {
        GameSettings.HUDCanvas.SetActive(false);
        GameSettings.PauseCanvas.SetActive(false);
        GameSettings.GameOverCanvas.SetActive(false);

        GameSettings.MenuCanvas.SetActive(true);
    }

    private static void ExitGame()
    {
        Application.Quit();
    }

}

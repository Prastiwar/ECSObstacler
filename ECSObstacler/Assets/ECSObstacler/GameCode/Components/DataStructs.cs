using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Transforms2D;

//  Players section  //

struct PlayerData
{
    public int Length;
    [ReadOnly] public ComponentDataArray<PlayerMarker> marker;
    [ReadOnly] public ComponentDataArray<Position2D> position;
    public ComponentDataArray<Health> health;
}

struct PlayerInputData
{
    public int Length;
    public ComponentDataArray<PlayerInput> input;
}

struct PlayerMoveData
{
    public int Length;
    [ReadOnly] public ComponentDataArray<TransformMatrix> matrix;
    [ReadOnly] public ComponentDataArray<MoveSpeed> speed;
    public ComponentDataArray<PlayerInput> input;
    public ComponentDataArray<Position2D> position;
}

struct PlayerPosData
{
    public int Length;
    [ReadOnly] public ComponentDataArray<PlayerMarker> marker;
    [ReadOnly] public ComponentDataArray<Position2D> position;
}

//  Obstacles section  //

struct ObstacleData
{
    public int Length;
    public ComponentDataArray<ObstacleMarker> marker;
    public ComponentDataArray<Position2D> position;
}

struct RemoveObstacleData
{
    public int Length;
    [ReadOnly] public EntityArray entity;
    [ReadOnly] public ComponentDataArray<ObstacleMarker> marker;
    [ReadOnly] public ComponentDataArray<Position2D> position;
}

struct ObstacleMoveData
{
    public int Length;
    public ComponentDataArray<Position2D> position;
    [ReadOnly] public ComponentDataArray<ObstacleMarker> marker;
    [ReadOnly] public ComponentDataArray<MoveSpeed> speed;
    [ReadOnly] public ComponentDataArray<Heading2D> head;
}

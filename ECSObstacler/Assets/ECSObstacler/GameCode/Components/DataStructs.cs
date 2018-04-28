using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Transforms2D;

struct PlayerInputData
{
    public int Length;
    public ComponentDataArray<PlayerInput> input;
}

struct MoveData
{
    public int Length;
    [ReadOnly] public ComponentDataArray<TransformMatrix> matrix;
    [ReadOnly] public ComponentDataArray<PlayerInput> input;
    [ReadOnly] public ComponentDataArray<MoveSpeed> speed;
    public ComponentDataArray<Position2D> position;
}

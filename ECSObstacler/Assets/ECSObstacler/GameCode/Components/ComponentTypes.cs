using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Health : IComponentData { public float Value; }
public struct MoveSpeed : IComponentData { public float Value; }
public struct ScoreGiver : IComponentData { public int Value; }
public struct ScoreHolder : IComponentData { public int Value; }
public struct ObstacleMarker : IComponentData { public bool1 MarkDead; }
public struct PlayerMarker : IComponentData { public bool1 MarkDead; }
public struct PlayerInput : IComponentData
{
    public float2 Velocity;
    public bool1 IsGrounded;
    public float2 Gravity;
}

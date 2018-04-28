﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Health : IComponentData { public float Value; }
public struct MoveSpeed : IComponentData { public float Value; }

public struct PlayerInput : IComponentData
{
    public float2 Move;
    public bool1 IsGrounded;
}
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

public class PlayerMoveSystem : ComponentSystem
{
    [Inject] MoveData posData;
    private float2 screenBorder;
    private bool1 isBorderSet;

    protected override void OnUpdate()
    {
        if (!isBorderSet)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            screenBorder = new float2(-planes[0].normal.x * planes[0].distance,
                                       -planes[1].normal.x * planes[1].distance);
            isBorderSet = true;
        }

        var dt = Time.deltaTime;

        for (int i = 0; i < posData.Length; i++)
        {
            var pos = posData.position[i];
            var scaleX = posData.matrix[i].Value.m0.x;
            var speedValue = dt * posData.speed[i].Value;
            var nextPos = pos.Value + (posData.input[i].Move * speedValue);

            pos.Value = math.lerp(pos.Value, nextPos, speedValue);
            pos.Value.x = math.clamp(pos.Value.x, (screenBorder.x + scaleX), (screenBorder.y - scaleX));

            posData.position[i] = pos;
        }
    }
}

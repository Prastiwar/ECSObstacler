using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

public struct JumpComponent : IComponentData { }

[UpdateAfter(typeof(PlayerInputSystem))]
public class PlayerMoveSystem : ComponentSystem
{
    [Inject] private PlayerMoveData data;

    protected override void OnUpdate()
    {
        var dt = Time.deltaTime;

        for (int i = 0; i < data.Length; i++)
        {
            var pos = data.position[i];
            var input = data.input[i];
            var scaleX = data.matrix[i].Value.m0.x;
            var scaleY = data.matrix[i].Value.m1.y;
            var speedValue = dt * data.speed[i].Value;
            var gravity = input.Gravity * dt;

            pos.Value = math.lerp(pos.Value, (pos.Value + gravity + input.Velocity), speedValue); // Apply gravity

            pos.Value.x = math.clamp(pos.Value.x, (ECSObstaclerBootstrap.ScreenBorder.x + scaleX), (ECSObstaclerBootstrap.ScreenBorder.y - scaleX));
            pos.Value.y = math.clamp(pos.Value.y, (ECSObstaclerBootstrap.ScreenBorder.z + scaleY), (ECSObstaclerBootstrap.ScreenBorder.w - scaleY));

            if (Utils.IsOnBorder(ECSObstaclerBootstrap.ScreenBorder, pos.Value, data.matrix[i]))
            {
                if (!input.IsGrounded)
                {
                    ToggleInput(input, i);
                }
            }
            else
            {
                if (input.IsGrounded)
                {
                    ToggleInput(input, i);
                }
            }
            data.position[i] = pos;
        }
    }

    private void ToggleInput(PlayerInput input, int i)
    {
        input.IsGrounded = !input.IsGrounded;
        data.input[i] = input;
    }
}

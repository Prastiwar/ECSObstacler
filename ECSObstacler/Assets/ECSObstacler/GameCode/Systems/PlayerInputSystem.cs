using UnityEngine;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem
{
    [Inject] PlayerInputData inputData;
    private bool isFirstFrameSet;

    protected override void OnUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        float gravityAmount = 9.81f;

        for (int i = 0; i < inputData.Length; i++)
        {
            var input = inputData.input[i];

            if (input.IsGrounded)
            {
                if (horizontalAxis > 0)
                    input.Gravity.x = gravityAmount;
                else if (horizontalAxis < 0)
                    input.Gravity.x = -gravityAmount;

                if (verticalAxis > 0)
                    input.Gravity.y = gravityAmount;
                else if (verticalAxis < 0)
                    input.Gravity.y = -gravityAmount;

                input.Velocity.x = horizontalAxis;
                input.Velocity.y = verticalAxis;
            }
            else
            {
                input.Velocity = 0;
            }

            inputData.input[i] = input;
        }
    }
}

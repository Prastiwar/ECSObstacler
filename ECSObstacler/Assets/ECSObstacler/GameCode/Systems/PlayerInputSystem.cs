using UnityEngine;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem
{
    struct PlayerInputData
    {
        public int Length;
        public ComponentDataArray<PlayerInput> input;
    }
    [Inject] PlayerInputData inputData;

    protected override void OnUpdate()
    {
        for (int i = 0; i < inputData.Length; i++)
        {
            var input = inputData.input[i];

            input.Move.x = Input.GetAxis("Horizontal");
            input.Move.y = Input.GetAxis("Vertical");

            inputData.input[i] = input;
        }
    }
}

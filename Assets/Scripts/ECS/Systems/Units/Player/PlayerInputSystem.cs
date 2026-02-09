using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class PlayerInputSystem : SystemBase
{
    private InputSystem_Actions _input;

    protected override void OnCreate()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    protected override void OnUpdate()
    {
        var directionInput = (float2)_input.Player.Move.ReadValue<Vector2>();
        foreach (var direction in SystemAPI.Query<RefRW<MoveDirection>>().WithAll<PlayerTag>()) 
        {
            direction.ValueRW.Value = directionInput;
        }
    }

    protected override void OnDestroy()
    {
        _input?.Disable();
        _input?.Dispose();
    }
}

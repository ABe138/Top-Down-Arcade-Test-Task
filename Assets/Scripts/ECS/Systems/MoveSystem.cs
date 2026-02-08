using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Physics.Systems;
using Unity.Physics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
public partial struct MoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, velocity, direction, moveData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PhysicsVelocity>, RefRO<MoveDirection>, RefRO<MoveData>>())
        {
            var desiredDirection = new float3(direction.ValueRO.Value.x, 0, direction.ValueRO.Value.y);
            var desiredVelocity = desiredDirection * moveData.ValueRO.MoveSpeed;
            velocity.ValueRW.Linear = desiredVelocity;

            if (math.lengthsq(desiredDirection) > 0.001f)
            {
                var targetRotation = quaternion.LookRotationSafe(desiredDirection, math.up());

                transform.ValueRW.Rotation = math.slerp(
                    transform.ValueRO.Rotation,
                    targetRotation,
                    moveData.ValueRO.RotationSpeed * deltaTime
                );
            }
        }
    }
}

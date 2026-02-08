using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyFollowPlayerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        if (SystemAPI.IsComponentEnabled<DestroyEntityFlag>(playerEntity)) return;

        var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        var enemyFollowPlayerJob = new EnemyFollowPlayerJob
        {
            PlayerPosition = playerTransform.ValueRO.Position.xz
        };

        state.Dependency = enemyFollowPlayerJob.ScheduleParallel(state.Dependency);
    }
}

[WithAll(typeof(EnemyTag))]
[BurstCompile]
public partial struct EnemyFollowPlayerJob : IJobEntity
{
    public float2 PlayerPosition;

    private void Execute(ref MoveDirection direction, in LocalTransform transform, in EnemyAttack attack)
    {
        var enemyPos = transform.Position.xz;
        var toPlayer = PlayerPosition - enemyPos;
        var distanceSq = math.lengthsq(toPlayer);

        if (distanceSq <= attack.AttackRange * attack.AttackRange)
        {
            direction.Value = float2.zero;
        }
        else
        {
            direction.Value = math.normalize(toPlayer);
        }
    }
}

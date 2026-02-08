using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemyFollowPlayerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        if (!SystemAPI.IsComponentEnabled<IsAlive>(playerEntity)) return;

        var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        var enemyFollowPlayerJob = new EnemyFollowPlayerJob
        {
            PlayerPosition = playerTransform.ValueRO.Position.xz
        };

        state.Dependency = enemyFollowPlayerJob.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
[WithAll(typeof(EnemyTag))]
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

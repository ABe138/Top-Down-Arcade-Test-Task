using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct ProjectileLifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (projectile, entity) in SystemAPI.Query<RefRW<ProjectileData>>().WithAll<ProjectileTag>().WithEntityAccess())
        {
            projectile.ValueRW.TimeRemaining -= deltaTime;

            if (projectile.ValueRO.TimeRemaining <= 0f)
            {
                SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);
            }
        }
    }
}

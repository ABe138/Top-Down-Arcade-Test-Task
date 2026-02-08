using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct ProjectileLifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (projectile, entity) in
            SystemAPI.Query<RefRW<ProjectileData>>()
                .WithAll<ProjectileTag>()
                .WithEntityAccess())
        {
            projectile.ValueRW.TimeRemaining -= deltaTime;

            if (projectile.ValueRO.TimeRemaining <= 0f)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[BurstCompile]
public partial struct ProjectileTriggerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        var projectileLookup = SystemAPI.GetComponentLookup<ProjectileData>(true);
        var enemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true);
        var isAliveLookup = SystemAPI.GetComponentLookup<IsAlive>(true);
        var damageBufferLookup = SystemAPI.GetBufferLookup<IncomingDamage>();

        state.Dependency = new ProjectileTriggerJob
        {
            ProjectileLookup = projectileLookup,
            EnemyLookup = enemyLookup,
            IsAliveLookup = isAliveLookup,
            DamageBufferLookup = damageBufferLookup,
            ECB = ecb.AsParallelWriter()
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public struct ProjectileTriggerJob : ITriggerEventsJob
{
    [ReadOnly] public ComponentLookup<ProjectileData> ProjectileLookup;
    [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
    [ReadOnly] public ComponentLookup<IsAlive> IsAliveLookup;
    public BufferLookup<IncomingDamage> DamageBufferLookup;
    public EntityCommandBuffer.ParallelWriter ECB;

    public void Execute(TriggerEvent triggerEvent)
    {
        var entityA = triggerEvent.EntityA;
        var entityB = triggerEvent.EntityB;

        // Determine which is projectile and which is enemy
        Entity projectileEntity;
        Entity enemyEntity;

        if (ProjectileLookup.HasComponent(entityA) && EnemyLookup.HasComponent(entityB))
        {
            projectileEntity = entityA;
            enemyEntity = entityB;
        }
        else if (ProjectileLookup.HasComponent(entityB) && EnemyLookup.HasComponent(entityA))
        {
            projectileEntity = entityB;
            enemyEntity = entityA;
        }
        else
        {
            return;
        }

        // Check if enemy is alive
        if (!IsAliveLookup.HasComponent(enemyEntity) || !IsAliveLookup.IsComponentEnabled(enemyEntity))
            return;

        // Apply damage
        var projectileData = ProjectileLookup[projectileEntity];
        if (DamageBufferLookup.HasBuffer(enemyEntity))
        {
            var damageBuffer = DamageBufferLookup[enemyEntity];
            damageBuffer.Add(new IncomingDamage { Value = (int)projectileData.Damage });
        }

        // Destroy projectile
        ECB.DestroyEntity(0, projectileEntity);
    }
}

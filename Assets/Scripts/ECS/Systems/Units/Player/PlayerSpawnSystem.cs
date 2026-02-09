using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Respawn>();
        state.RequireForUpdate<PlayerSpawnData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (spawnTag, spawnData, entity) in SystemAPI.Query<RefRW<Respawn>, RefRO<PlayerSpawnData>>().WithEntityAccess())
        {
            var player = ecb.Instantiate(spawnData.ValueRO.SpawnEntity);
            ecb.SetComponent(player, LocalTransform.FromPosition(float3.zero));
            ecb.SetComponentEnabled<Respawn>(entity, false);
        }
    }
}
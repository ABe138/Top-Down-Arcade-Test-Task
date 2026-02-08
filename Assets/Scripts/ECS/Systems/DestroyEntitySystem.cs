using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(DamageProcessingSystem))]
public partial struct DestroyUnitSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (_, entity) in SystemAPI.Query<RefRO<IsAlive>>().WithDisabled<IsAlive>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
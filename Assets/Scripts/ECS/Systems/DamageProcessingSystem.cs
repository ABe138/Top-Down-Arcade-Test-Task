using Unity.Burst;
using Unity.Entities;

public partial struct DamageProcessingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (hitPoints, damageBuffer, entity) in SystemAPI.Query<RefRW<CurrentHitPoints>, DynamicBuffer<IncomingDamage>>().WithDisabled<DestroyEntityFlag>().WithEntityAccess())
        {
            foreach (var damage in damageBuffer)
            {
                hitPoints.ValueRW.Value -= damage.Value;
            }
            damageBuffer.Clear();

            if (hitPoints.ValueRO.Value <= 0) 
            {
                SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);
            }
        }
    }
}

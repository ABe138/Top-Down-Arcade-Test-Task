using Unity.Entities;

[UpdateAfter(typeof(DamageProcessingSystem))]
[UpdateBefore(typeof(DestroyEntitySystem))]
public partial struct PlayerHealthUISystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (PlayerHUDManager.Instance == null) return;
        foreach (var (currentHealth, maxHealth, updateFlag) in SystemAPI.Query<RefRO<CurrentHitPoints>, RefRO<MaxHitPoints>, EnabledRefRW<UpdateHealthUIFlag>>())
        {
            PlayerHUDManager.Instance.UpdatePlayerHealthText(currentHealth.ValueRO.Value, maxHealth.ValueRO.Value);
            updateFlag.ValueRW = false;
        }
    }
}
